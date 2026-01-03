using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenGL_Game.Engine.Managers
{
    public static class HighscoreManager
    {
        private static int port;
        private static TcpClient client;
        private static StreamReader reader;
        private static StreamWriter writer;
        private static List<(string, int)> HighScores = new List<(string, int)>();
        public static (string, int) HighScore = ("blank",0);

        public static void Initalise()
        {
            port = 8080;
            client = new TcpClient("localhost", port);
            NetworkStream stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };

            LoadHighScores();
        }

        public static void LoadHighScores()
        {
            if (writer == null) Initalise();

            // Request highscores from the server
            writer.WriteLine("GET_HIGHSCORES");
            string response = reader.ReadLine();
            // Example response: "Alice,1000;Bob,900;Carol,800"
            if (!string.IsNullOrWhiteSpace(response))
            {
                HighScores.Clear();
                var entries = response.Split(';', StringSplitOptions.RemoveEmptyEntries);
                foreach (var entry in entries)
                {
                    var parts = entry.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                    {
                        HighScores.Add((parts[0], score));
                    }
                }
                // Ensure sorted after loading
                HighScores = HighScores.OrderByDescending(hs => hs.Item2).ToList();
            }
        }

        public static List<(string, int)> GetHighScores()
        {
            if (writer == null) Initalise();
            return HighScores;
        }

        public static void AddHighscore((string,int)Highscore)
        {
            if (writer == null) Initalise();

            HighScores.Add(Highscore);
            // Sort descending by score
            HighScores = HighScores.OrderByDescending(hs => hs.Item2).ToList();

            if (HighScores.Count > 5)
            {
                HighScores.RemoveAt(HighScores.Count - 1);
            }
            SendHighscoresToServer();
        }
        public static void AddNewScore(int score)
        {
            HighScore.Item2 = score;
        }
        public static void AddToScore(int score)
        {
            HighScore.Item2 += score;
        }
        public static int GetCurrentScore()
        {
            return HighScore.Item2;
        }
        public static void AddNewName(string playerName)
        {
            HighScore.Item1 = playerName;
        }

        public static void SendHighscoresToServer()
        {
            if (writer == null) Initalise();

            // Send all highscores to the server as a batch
            // Example protocol: "SUBMIT_HIGHSCORES:Alice,1000;Bob,900;Carol,800"
            var highscoreString = string.Join(";", HighScores.Select(hs => $"{hs.Item1},{hs.Item2}"));
            writer.WriteLine($"SUBMIT_HIGHSCORES:{highscoreString}");

            // Optionally, read server response (could be updated list or confirmation)
            string response = reader.ReadLine();
            // You can handle the response as needed
        }

        public static void Close()
        {
            if(writer == null) return;

            writer.WriteLine("CLOSE_SERVER");
            writer?.Dispose();
            reader?.Dispose();
            client?.Close();
        }

    }
}
