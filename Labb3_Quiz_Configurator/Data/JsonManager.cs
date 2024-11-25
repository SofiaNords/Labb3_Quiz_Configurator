using Labb3_Quiz_Configurator.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Labb3_Quiz_Configurator
{
    public class JsonManager
    {
        private readonly string _filePath;

        public JsonManager()
        {
            string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Steg 2: Skapa mappen Quiz_labb om den inte redan finns
            string quizLabbFolderPath = Path.Combine(localAppDataPath, "Quiz_labb");
            if (!Directory.Exists(quizLabbFolderPath))
            {
                Directory.CreateDirectory(quizLabbFolderPath);
                Console.WriteLine("Mappen 'Quiz_labb' har skapats.");
            }

            // Steg 3: Sätt sökvägen till QuestionPacks-filen
            _filePath = Path.Combine(quizLabbFolderPath, "QuestionPacks.json");

            // Steg 4: Skapa filen om den inte finns
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
                Console.WriteLine("Filen 'QuestionPacks' har skapats.");
            }

        }

        // Läs quizpaket från en JSON-fil
        public async Task<List<QuestionPack>> LoadQuestionPacks()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = await File.ReadAllTextAsync(_filePath);
                    return JsonSerializer.Deserialize<List<QuestionPack>>(json) ?? new List<QuestionPack>();
                }
                else
                {
                    return new List<QuestionPack>(); // kan returnera Null istället
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid inläsning av JSON: {ex.Message}");
                return new List<QuestionPack>(); // kan returnera Null istället
            }
        }

        // Spara quizpaket till en JSON-fil
        public async Task SaveQuestionPacks(List<QuestionPack> questionPacks)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true // För att få finare formaterad JSON
                };

                string json = JsonSerializer.Serialize(questionPacks, options);
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid sparande av JSON: {ex.Message}");
            }
        }
    }
}
