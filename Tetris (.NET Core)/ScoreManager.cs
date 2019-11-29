using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Tetris
{
    public class ScoreManager
    {
        private readonly string highScoreFile;

        private readonly int[] ScorePerLines = { 1, 40, 100, 300, 1200 };

        public ScoreManager(string highScoreFile)
        {
            this.highScoreFile = highScoreFile;
            this.HighScore = this.GetHighScore();
        }

        public int Score { get; private set; }

        public int HighScore { get; private set; }

        public void AddToScore(int level, int lines)
        {
            this.Score += ScorePerLines[lines] * level;
            if (this.Score > this.HighScore)
            {
                this.HighScore = this.Score;
            }
        }

        public void AddToHighScore()
        {
            File.AppendAllLines(this.highScoreFile, new List<string>
                        {
                            $"[{DateTime.Now.ToString()}] {Environment.UserName} => {this.Score}"
                        });
        }

        private int GetHighScore()
        {
            var highScore = 0;
            if (File.Exists(this.highScoreFile))
            {
                var allScores = File.ReadAllLines(this.highScoreFile);
                foreach (var score in allScores)
                {
                    var match = Regex.Match(score, @" => (?<score>[0-9]+)");
                    highScore = Math.Max(highScore, int.Parse(match.Groups["score"].Value));
                }
            }

            return highScore;
        }
    }
}
