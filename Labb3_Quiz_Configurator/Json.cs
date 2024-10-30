//using System.IO;
//using System.Text.Json;

//var data = new { FirstName = "Kalle", LastName = "Andersson", Length = 182 };

//// För att få JSON koden finare formaterad
//var options = new JsonSerializerOptions() { WriteIndented = true };

//string json = JsonSerializer.Serialize(data, options);

//Console.WriteLine(json);

//File.WriteAllText("demo.json", json);

//string json = File.ReadAllText("people.json");

//Console.WriteLine(json);

//var people = JsonSerializer.Deserialize<List<Person>>(json);

// En klass med en Load och en Save metod