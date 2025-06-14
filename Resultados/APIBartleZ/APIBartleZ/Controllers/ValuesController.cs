using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.SqlServer.Server;
using NuGet.Versioning;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIBartleZ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost("PreQuest")]
        [EnableCors("AllowAllOrigins")]
        public ActionResult Post([FromBody] Player player)
        {
            try
            {
                // Define the directory where the JSON files will be stored
                string folderPath = "C:/Users/djpmg/Desktop/Results/PreQuest";

                // Ensure the folder exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Generate the file name based on PNumber
                string fileName = $"Player{player.PNumber}.json";
                string filePath = Path.Combine(folderPath, fileName);

                // Serialize the player object to JSON
                string jsonData = JsonSerializer.Serialize(player, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                // Write the JSON data to the file
                System.IO.File.WriteAllText(filePath, jsonData);

                return Ok($"Player data saved to {filePath}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error saving player data: {ex.Message}");
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        // POST api/<ValuesController>
        [HttpPost("ScaledQuiz")]
        [EnableCors("AllowAllOrigins")]
        public ActionResult Post([FromBody] ScaledQuiz quiz)
        {
            try
            {
                // Descobrir o Dominant Type

                // Q1 
                if(quiz.Q1.Equals("Q1O1"))
                {
                    quiz.SCount += 1;
                }
                else if (quiz.Q1.Equals("Q1O2"))
                {
                    quiz.ACount += 1;
                }

                // Q2 
                if (quiz.Q2.Equals("Q2O1"))
                {
                    quiz.ACount += 1;
                }
                else if (quiz.Q2.Equals("Q2O2"))
                {
                    quiz.SCount += 1;
                }

                // Q3 
                if (quiz.Q3.Equals("Q3O1"))
                {
                    quiz.SCount += 1;
                }
                else if (quiz.Q3.Equals("Q3O2"))
                {
                    quiz.ACount += 1;
                }

                // Q4 
                if (quiz.Q4.Equals("Q4O1"))
                {
                    quiz.ACount += 1;
                }
                else if (quiz.Q4.Equals("Q4O2"))
                {
                    quiz.SCount += 1;
                }

                // Q5 
                if (quiz.Q5.Equals("Q5O1"))
                {
                    quiz.SCount += 1;
                }
                else if (quiz.Q5.Equals("Q5O2"))
                {
                    quiz.ACount += 1;
                }

                // Q6 
                if (quiz.Q6.Equals("Q6O1"))
                {
                    quiz.SCount += 1;
                }
                else if (quiz.Q6.Equals("Q6O2"))
                {
                    quiz.ACount += 1;
                }

                // Q7 
                if (quiz.Q7.Equals("Q7O1"))
                {
                    quiz.SCount += 1;
                }
                else if (quiz.Q7.Equals("Q7O2"))
                {
                    quiz.ACount += 1;
                }

                // Q8 
                if (quiz.Q8.Equals("Q8O1"))
                {
                    quiz.SCount += 1;
                }
                else if (quiz.Q8.Equals("Q8O2"))
                {
                    quiz.ECount += 1;
                }

                // Q9
                if (quiz.Q9.Equals("Q9O1"))
                {
                    quiz.SCount += 1;
                }
                else if (quiz.Q9.Equals("Q9O2"))
                {
                    quiz.ECount += 1;
                }

                // Q10
                if (quiz.Q10.Equals("Q10O1"))
                {
                    quiz.SCount += 1;
                }
                else if (quiz.Q10.Equals("Q10O2"))
                {
                    quiz.ECount += 1;
                }

                // Q11
                if (quiz.Q11.Equals("Q11O1"))
                {
                    quiz.SCount += 1;
                }
                else if (quiz.Q11.Equals("Q11O2"))
                {
                    quiz.ECount += 1;
                }

                // Q12
                if (quiz.Q12.Equals("Q12O1"))
                {
                    quiz.SCount += 1;
                }
                else if (quiz.Q12.Equals("Q12O2"))
                {
                    quiz.ECount += 1;
                }

                // Q13
                if (quiz.Q13.Equals("Q13O1"))
                {
                    quiz.SCount += 1;
                }
                else if (quiz.Q13.Equals("Q13O2"))
                {
                    quiz.ECount += 1;
                }

                // Q14
                if (quiz.Q14.Equals("Q14O1"))
                {
                    quiz.KCount += 1;
                }
                else if (quiz.Q14.Equals("Q14O2"))
                {
                    quiz.SCount += 1;
                }

                // Q15
                if (quiz.Q15.Equals("Q15O1"))
                {
                    quiz.SCount += 1;
                }
                else if (quiz.Q15.Equals("Q15O2"))
                {
                    quiz.KCount += 1;
                }

                // Q16
                if (quiz.Q16.Equals("Q16O1"))
                {
                    quiz.SCount += 1;
                }
                else if (quiz.Q16.Equals("Q16O2"))
                {
                    quiz.KCount += 1;
                }

                // Q17
                if (quiz.Q17.Equals("Q17O1"))
                {
                    quiz.KCount += 1;
                }
                else if (quiz.Q17.Equals("Q17O2"))
                {
                    quiz.SCount += 1;
                }

                // Q18
                if (quiz.Q18.Equals("Q18O1"))
                {
                    quiz.KCount += 1;
                }
                else if (quiz.Q18.Equals("Q18O2"))
                {
                    quiz.SCount += 1;
                }

                // Q19
                if (quiz.Q19.Equals("Q19O1"))
                {
                    quiz.KCount += 1;
                }
                else if (quiz.Q19.Equals("Q19O2"))
                {
                    quiz.SCount += 1;
                }

                // Q20
                if (quiz.Q20.Equals("Q20O1"))
                {
                    quiz.SCount += 1;
                }
                else if (quiz.Q20.Equals("Q20O2"))
                {
                    quiz.KCount += 1;
                }

                // Q21
                if (quiz.Q21.Equals("Q21O1"))
                {
                    quiz.ECount += 1;
                }
                else if (quiz.Q21.Equals("Q21O2"))
                {
                    quiz.ACount += 1;
                }

                // Q22
                if (quiz.Q22.Equals("Q22O1"))
                {
                    quiz.ECount += 1;
                }
                else if (quiz.Q22.Equals("Q22O2"))
                {
                    quiz.ACount += 1;
                }

                // Q23
                if (quiz.Q23.Equals("Q23O1"))
                {
                    quiz.ACount += 1;
                }
                else if (quiz.Q23.Equals("Q23O2"))
                {
                    quiz.ECount += 1;
                }

                // Q24
                if (quiz.Q24.Equals("Q24O1"))
                {
                    quiz.ECount += 1;
                }
                else if (quiz.Q24.Equals("Q24O2"))
                {
                    quiz.ACount += 1;
                }

                // Q25
                if (quiz.Q25.Equals("Q25O1"))
                {
                    quiz.ECount += 1;
                }
                else if (quiz.Q25.Equals("Q25O2"))
                {
                    quiz.ACount += 1;
                }

                // Q26
                if (quiz.Q26.Equals("Q26O1"))
                {
                    quiz.ECount += 1;
                }
                else if (quiz.Q26.Equals("Q26O2"))
                {
                    quiz.ACount += 1;
                }

                // Q27
                if (quiz.Q27.Equals("Q27O1"))
                {
                    quiz.ECount += 1;
                }
                else if (quiz.Q27.Equals("Q27O2"))
                {
                    quiz.KCount += 1;
                }

                // Q28
                if (quiz.Q28.Equals("Q28O1"))
                {
                    quiz.ECount += 1;
                }
                else if (quiz.Q28.Equals("Q28O2"))
                {
                    quiz.KCount += 1;
                }

                // Q29
                if (quiz.Q29.Equals("Q29O1"))
                {
                    quiz.ECount += 1;
                }
                else if (quiz.Q29.Equals("Q29O2"))
                {
                    quiz.KCount += 1;
                }

                // Q30
                if (quiz.Q30.Equals("Q30O1"))
                {
                    quiz.ECount += 1;
                }
                else if (quiz.Q30.Equals("Q30O2"))
                {
                    quiz.KCount += 1;
                }

                // Q31
                if (quiz.Q31.Equals("Q31O1"))
                {
                    quiz.KCount += 1;
                }
                else if (quiz.Q31.Equals("Q31O2"))
                {
                    quiz.ECount += 1;
                }

                // Q32
                if (quiz.Q32.Equals("Q32O1"))
                {
                    quiz.ECount += 1;
                }
                else if (quiz.Q32.Equals("Q32O2"))
                {
                    quiz.KCount += 1;
                }

                // Q33
                if (quiz.Q33.Equals("Q33O1"))
                {
                    quiz.ECount += 1;
                }
                else if (quiz.Q33.Equals("Q33O2"))
                {
                    quiz.KCount += 1;
                }

                // Q34
                if (quiz.Q34.Equals("Q34O1"))
                {
                    quiz.ACount += 1;
                }
                else if (quiz.Q34.Equals("Q34O2"))
                {
                    quiz.KCount += 1;
                }

                // Q35
                if (quiz.Q35.Equals("Q35O1"))
                {
                    quiz.KCount += 1;
                }
                else if (quiz.Q35.Equals("Q35O2"))
                {
                    quiz.ACount += 1;
                }

                // Q36
                if (quiz.Q36.Equals("Q36O1"))
                {
                    quiz.KCount += 1;
                }
                else if (quiz.Q36.Equals("Q36O2"))
                {
                    quiz.ACount += 1;
                }

                // Q37
                if (quiz.Q37.Equals("Q37O1"))
                {
                    quiz.ACount += 1;
                }
                else if (quiz.Q37.Equals("Q37O2"))
                {
                    quiz.KCount += 1;
                }

                // Q38
                if (quiz.Q38.Equals("Q38O1"))
                {
                    quiz.ACount += 1;
                }
                else if (quiz.Q38.Equals("Q38O2"))
                {
                    quiz.KCount += 1;
                }

                // Q39
                if (quiz.Q39.Equals("Q39O1"))
                {
                    quiz.ACount += 1;
                }
                else if (quiz.Q39.Equals("Q39O2"))
                {
                    quiz.KCount += 1;
                }

                // Calculate the percentages

                quiz.APercentage = (float)quiz.ACount / (float)39;
                quiz.KPercentage = (float)quiz.KCount / (float)39;
                quiz.SPercentage = (float)quiz.SCount / (float)39;
                quiz.EPercentage = (float)quiz.ECount / (float)39;

                // Get Dominant Type

                List<string> dominantTypes = new List<string>();


                float max = Math.Max(Math.Max(quiz.ACount, quiz.KCount), Math.Max(quiz.SCount, quiz.ECount));
                
                if (quiz.ACount == max) dominantTypes.Add("Achiever");
                if (quiz.KCount == max) dominantTypes.Add("Killer");
                if (quiz.SCount == max) dominantTypes.Add("Socializer");
                if (quiz.ECount == max) dominantTypes.Add("Explorer");

                quiz.DominantType = string.Join(", ", dominantTypes);


                // Define the directory where the JSON files will be stored
                string folderPath = "C:/Users/djpmg/Desktop/Results/ScaledQuiz";

                // Ensure the folder exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Generate the file name based on PNumber
                string fileName = $"Player{quiz.PNumber}.json";
                string filePath = Path.Combine(folderPath, fileName);

                // Serialize the player object to JSON
                string jsonData = JsonSerializer.Serialize(quiz, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                // Write the JSON data to the file
                System.IO.File.WriteAllText(filePath, jsonData);

                return Ok($"Player data saved to {filePath}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error saving player data: {ex.Message}");
            }
        }

        [HttpPost("GameInfo")]
        [EnableCors("AllowAllOrigins")]
        public ActionResult Post([FromBody] GameInfo game)
        {
            try
            {
                // Define the directory where the JSON files will be stored
                string folderPath = "C:/Users/djpmg/Desktop/Results/GameInfo";

                // Ensure the folder exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Generate the file name based on PNumber
                string fileName = $"Player{game.PNumber}.json";
                string filePath = Path.Combine(folderPath, fileName);

                // Serialize the player object to JSON
                string jsonData = JsonSerializer.Serialize(game, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                // Write the JSON data to the file
                System.IO.File.WriteAllText(filePath, jsonData);

                return Ok($"Player data saved to {filePath}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error saving player data: {ex.Message}");
            }
        }

        [HttpPost("PostQuest")]
        [EnableCors("AllowAllOrigins")]
        public ActionResult Post([FromBody] PostQuest game)
        {
            try
            {
                // Define the directory where the JSON files will be stored
                string folderPath = "C:/Users/djpmg/Desktop/Results/PostQuest";

                // Ensure the folder exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Generate the file name based on PNumber
                string fileName = $"Player{game.pNumber}.json";
                string filePath = Path.Combine(folderPath, fileName);

                // Serialize the player object to JSON
                string jsonData = JsonSerializer.Serialize(game, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                // Write the JSON data to the file
                System.IO.File.WriteAllText(filePath, jsonData);

                return Ok($"Player data saved to {filePath}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error saving player data: {ex.Message}");
            }
        }

        // POST api/<ValuesController>
        [HttpPost("ScaledQuiz30")]
        [EnableCors("AllowAllOrigins")]
        public ActionResult Post([FromBody] ScaledQuiz30 quiz)
        {
            try
            {
                // Descobrir o Dominant Type

                /*

                if (quiz.Q1 == "Q1O1") quiz.ACount += 1;
                else if (quiz.Q1 == "Q1O2") quiz.SCount += 1;

                if (quiz.Q2 == "Q2O1") quiz.SCount += 1;
                else if (quiz.Q2 == "Q2O2") quiz.ACount += 1;

                if (quiz.Q3 == "Q3O1") quiz.SCount += 1;
                else if (quiz.Q3 == "Q3O2") quiz.ACount += 1;

                if (quiz.Q4 == "Q4O1") quiz.SCount += 1;
                else if (quiz.Q4 == "Q4O2") quiz.ACount += 1;

                if (quiz.Q5 == "Q5O1") quiz.SCount += 1;
                else if (quiz.Q5 == "Q5O2") quiz.ACount += 1;

                if (quiz.Q6 == "Q6O1") quiz.SCount += 1;
                else if (quiz.Q6 == "Q6O2") quiz.ECount += 1;

                if (quiz.Q7 == "Q7O1") quiz.SCount += 1;
                else if (quiz.Q7 == "Q7O2") quiz.ECount += 1;

                if (quiz.Q8 == "Q8O1") quiz.SCount += 1;
                else if (quiz.Q8 == "Q8O2") quiz.ECount += 1;

                if (quiz.Q9 == "Q9O1") quiz.SCount += 1;
                else if (quiz.Q9 == "Q9O2") quiz.ECount += 1;

                if (quiz.Q10 == "Q10O1") quiz.SCount += 1;
                else if (quiz.Q10 == "Q10O2") quiz.ECount += 1;

                if (quiz.Q11 == "Q11O1") quiz.KCount += 1;
                else if (quiz.Q11 == "Q11O2") quiz.SCount += 1;

                if (quiz.Q12 == "Q12O1") quiz.SCount += 1;
                else if (quiz.Q12 == "Q12O2") quiz.KCount += 1;

                if (quiz.Q13 == "Q13O1") quiz.KCount += 1;
                else if (quiz.Q13 == "Q13O2") quiz.SCount += 1;

                if (quiz.Q14 == "Q14O1") quiz.KCount += 1;
                else if (quiz.Q14 == "Q14O2") quiz.SCount += 1;

                if (quiz.Q15 == "Q15O1") quiz.KCount += 1;
                else if (quiz.Q15 == "Q15O2") quiz.SCount += 1;

                if (quiz.Q16 == "Q16O1") quiz.ECount += 1;
                else if (quiz.Q16 == "Q16O2") quiz.ACount += 1;

                if (quiz.Q17 == "Q17O1") quiz.ECount += 1;
                else if (quiz.Q17 == "Q17O2") quiz.ACount += 1;

                if (quiz.Q18 == "Q18O1") quiz.ACount += 1;
                else if (quiz.Q18 == "Q18O2") quiz.ECount += 1;

                if (quiz.Q19 == "Q19O1") quiz.ECount += 1;
                else if (quiz.Q19 == "Q19O2") quiz.ACount += 1;

                if (quiz.Q20 == "Q20O1") quiz.ECount += 1;
                else if (quiz.Q20 == "Q20O2") quiz.ACount += 1;

                if (quiz.Q21 == "Q21O1") quiz.ECount += 1;
                else if (quiz.Q21 == "Q21O2") quiz.KCount += 1;

                if (quiz.Q22 == "Q22O1") quiz.ECount += 1;
                else if (quiz.Q22 == "Q22O2") quiz.KCount += 1;

                if (quiz.Q23 == "Q23O1") quiz.KCount += 1;
                else if (quiz.Q23 == "Q23O2") quiz.ECount += 1;

                if (quiz.Q24 == "Q24O1") quiz.ECount += 1;
                else if (quiz.Q24 == "Q24O2") quiz.KCount += 1;

                if (quiz.Q25 == "Q25O1") quiz.ECount += 1;
                else if (quiz.Q25 == "Q25O2") quiz.KCount += 1;

                if (quiz.Q26 == "Q26O1") quiz.ACount += 1;
                else if (quiz.Q26 == "Q26O2") quiz.KCount += 1;

                if (quiz.Q27 == "Q27O1") quiz.KCount += 1;
                else if (quiz.Q27 == "Q27O2") quiz.ACount += 1;

                if (quiz.Q28 == "Q28O1") quiz.KCount += 1;
                else if (quiz.Q28 == "Q28O2") quiz.ACount += 1;

                if (quiz.Q29 == "Q29O1") quiz.ACount += 1;
                else if (quiz.Q29 == "Q29O2") quiz.KCount += 1;

                if (quiz.Q30 == "Q30O1") quiz.ACount += 1;
                else if (quiz.Q30 == "Q30O2") quiz.KCount += 1;

                // Calculate the percentages

                quiz.APercentage = (float)quiz.ACount / (float)30;
                quiz.KPercentage = (float)quiz.KCount / (float)30;
                quiz.SPercentage = (float)quiz.SCount / (float)30;
                quiz.EPercentage = (float)quiz.ECount / (float)30;

                // Get Dominant Type

                List<string> dominantTypes = new List<string>();


                float max = Math.Max(Math.Max(quiz.ACount, quiz.KCount), Math.Max(quiz.SCount, quiz.ECount));

                if (quiz.ACount == max) dominantTypes.Add("Achiever");
                if (quiz.KCount == max) dominantTypes.Add("Killer");
                if (quiz.SCount == max) dominantTypes.Add("Socializer");
                if (quiz.ECount == max) dominantTypes.Add("Explorer");

                quiz.DominantType = string.Join(", ", dominantTypes);
                */

                // Define the directory where the JSON files will be stored
                string folderPath = "C:/Users/djpmg/Desktop/Results/ScaledQuiz";

                // Ensure the folder exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Generate the file name based on PNumber
                string fileName = $"Player{quiz.PNumber}.json";
                string filePath = Path.Combine(folderPath, fileName);

                // Serialize the player object to JSON
                string jsonData = JsonSerializer.Serialize(quiz, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                // Write the JSON data to the file
                System.IO.File.WriteAllText(filePath, jsonData);

                return Ok($"Player data saved to {filePath}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error saving player data: {ex.Message}");
            }
        }

        [HttpPost("HITStandard")]
        [EnableCors("AllowAllOrigins")]
        public ActionResult Post([FromBody] HITStandard game)
        {
            try
            {
                // Define the directory where the JSON files will be stored
                string folderPath = "C:/Users/djpmg/Desktop/Results/HITStandard";

                // Ensure the folder exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Generate the file name based on PNumber
                string fileName = $"Player{game.playerNumber}.json";
                string filePath = Path.Combine(folderPath, fileName);

                // Serialize the player object to JSON
                string jsonData = JsonSerializer.Serialize(game, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                // Write the JSON data to the file
                System.IO.File.WriteAllText(filePath, jsonData);

                return Ok($"Player data saved to {filePath}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error saving player data: {ex.Message}");
            }
        }
    }
}
