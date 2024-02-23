namespace CommandService
{
    public static class DotEnv
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var env = line.Split(
                    '=',
                    StringSplitOptions.RemoveEmptyEntries);

                if (env.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(env[0], env[1]);
            }
        }
    }
}