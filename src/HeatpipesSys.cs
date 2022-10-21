using System.Collections.Generic;
using Game.Components;
using Game.Constants;
using Game.Data;
using Game.Systems;
using KL.Utils;
using UnityEngine;

namespace Lunar.Exchange.Mods.Stardeus.Heatpipes {
    public sealed class HeatpipesSys : GameSystem, ISaveable {
        // The convention is that all systems end with Sys, and SysId is equal to the class name
        public const string SysId = nameof(HeatpipesSys);
        public override string Id => SysId;

        // If your system can work in sandbox too, set this to false
        public override bool SkipInSandbox => false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Register() {
            GameSystems.Register(SysId, () => new HeatpipesSys());
        }

        // If your system adds some mechanic that may not be compatible with
        // old saves, you have to set the MinRequiredVersion
        //public override string MinRequiredVersion => "0.6.89";

        private GameState state;

        // By returning something in the Overlays list, your system can add
        // buttons in the top right section of the game UI.
        // Those buttons will toggle the overlays.
        // Almost always there will be just one OverlayInfo in the list,
        // The only exceptions right now are:
        // - EquilibriumSys (Oxygen, Heat, Airtightness and Insulation)
        // - HiveMindSys (Beings, Tasks)

        //private ExampleOverlayUI ui;
        // Public so that the UI can use it
        public int SomeVariable;

        protected override void OnInitialize() {
            S.Sig.AfterLoadState.AddListener(OnLoadSave);
            S.Sig.AreasInitialized.AddListener(OnAreasInit);
        }

        private void OnLoadSave(GameState state) {
            this.state = state;
            S.Clock.OnTick.AddListener(OnTick);
        }

        // If your system depends on AreasSys, for example, you may want to
        // start ticking your system only after initial areas have been built
        private void OnAreasInit() {
            S.Clock.OnTickAsync.AddListener(OnTickAsync);
        }

        // OnTickAsync should be prefered over this
        private void OnTick(long ticks) {
            D.Warn("Ticking synchronously. Tick: {0}", ticks);
        }

        // If the simulation cannot keep up, delta will show how many ticks have been skipped since last call
        private void OnTickAsync(long ticks, int delta) {
            D.Warn("Ticking asynchronously. Tick: {0}, Delta: {1}", ticks, delta);
        }

        public override void Unload() {
            // Release the resources here
        }

        public string GetName() {
            return Id;
        }

        // We use ComponentData for saving entity components, but it can be
        // used in systems as well, if the system is marked ISaveable
        private ComponentData data;

        public ComponentData Save() {
            data ??= new ComponentData(0, SysId);
            data.SetInt("SomeVariable", SomeVariable);
            return data;
        }

        public void Load(ComponentData data) {
            this.data = data;
            SomeVariable = data.GetInt("SomeVariable", 0);
        }
    }
}