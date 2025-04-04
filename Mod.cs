using Colossal.IO.AssetDatabase;
using Colossal.Serialization.Entities;
using DisableAccidents.ModSettings;
using Game;
using Game.Modding;
using Game.Prefabs;
using Game.SceneFlow;
using Unity.Entities;

namespace DisableAccidents
{
    public class Mod : IMod
    {
        internal static Setting m_Setting;

        public void OnLoad(UpdateSystem updateSystem)
        {
            LogUtil.Info("OnLoad");

            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
            AssetDatabase.global.LoadSettings(nameof(DisableAccidents), m_Setting);
            m_Setting.Apply();

            updateSystem.UpdateAt<NoTrafficAccidentsSystem>(SystemUpdatePhase.ModificationEnd);
            updateSystem.UpdateAt<InitialSettingsApplySystem>(SystemUpdatePhase.ModificationEnd);
        }

        public void OnDispose()
        {
            LogUtil.Info("OnDispose");
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
        }
    }

    public partial class InitialSettingsApplySystem : GameSystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            this.Enabled = false;
        }

        protected override void OnUpdate()
        {
            if (Setting.Instance != null)
            {
                var accidentSystem = EntityManager.World.GetOrCreateSystemManaged<NoTrafficAccidentsSystem>();
                accidentSystem.ForceUpdate(
                    Setting.Instance.ModEnabled,
                    Setting.Instance.ModEnabled ? 0f : (Setting.Instance.AccidentProbability / 100f)
                );
                this.Enabled = false;
            }
        }
    }

    public partial class NoTrafficAccidentsSystem : GameSystemBase
    {
        private EntityQuery m_AccidentPrefabQuery;
        private bool m_LastEnabledState;
        private float m_LastProbability;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_AccidentPrefabQuery = GetEntityQuery(
                ComponentType.ReadOnly<EventData>(),
                ComponentType.ReadOnly<TrafficAccidentData>(),
                ComponentType.Exclude<Locked>()
            );

            RequireForUpdate(m_AccidentPrefabQuery);
        }

        public void ForceUpdate(bool currentEnabled, float probability)
        {
            m_LastEnabledState = currentEnabled;
            m_LastProbability = probability;
            UpdateAccidentProbabilities(currentEnabled, probability);
        }

        protected override void OnUpdate()
        {
            bool currentEnabled = Setting.Instance?.ModEnabled ?? false;
            float currentProbability = currentEnabled ? 0f : (Setting.Instance?.AccidentProbability ?? 100) / 100f;

            if (currentEnabled != m_LastEnabledState || currentProbability != m_LastProbability)
            {
                m_LastEnabledState = currentEnabled;
                m_LastProbability = currentProbability;
                UpdateAccidentProbabilities(currentEnabled, currentProbability);
            }
        }

        private void UpdateAccidentProbabilities(bool disableAccidents, float probability)
        {
            LogUtil.Info($"Setting accident probabilities to {(disableAccidents ? "0" : probability.ToString())}");
            var accidentPrefabs = m_AccidentPrefabQuery.ToEntityArray(World.UpdateAllocator.ToAllocator);
            foreach (var prefab in accidentPrefabs)
            {
                if (EntityManager.HasComponent<TrafficAccidentData>(prefab))
                {
                    var accidentData = EntityManager.GetComponentData<TrafficAccidentData>(prefab);
                    accidentData.m_OccurenceProbability = disableAccidents ? 0f : probability;
                    EntityManager.SetComponentData(prefab, accidentData);
                }
            }
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);
            bool currentEnabled = Setting.Instance?.ModEnabled ?? false;
            float currentProbability = currentEnabled ? 0f : (Setting.Instance?.AccidentProbability ?? 100) / 100f;
            ForceUpdate(currentEnabled, currentProbability);
        }
    }
}