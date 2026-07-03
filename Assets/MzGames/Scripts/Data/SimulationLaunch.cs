namespace MzGames.Scripts.Data
{
    public   class SimulationLaunch
    {
        public SimulationConfig Config;
        public SimulationSnapshot Snapshot;

        public bool IsResume => Snapshot != null;

        public static SimulationLaunch New(SimulationConfig config) =>
            new SimulationLaunch { Config = config };

        public static SimulationLaunch Resume(SimulationSnapshot snapshot) =>
            new SimulationLaunch { Config = snapshot.Config, Snapshot = snapshot };
    }
}
