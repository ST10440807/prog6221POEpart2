using System;
using System.Collections.Generic;//Enables dictionary usage*
using System.IO;//Handles file operations
using System.Linq;
using System.Media;//Enables audio playback
using System.Text;
using System.Threading.Tasks;
/* Chatbot for cyber-security awareness
    * Phishing scams,malware and social engineering,discuss safe password practises and recognizing suspicious links
    */

namespace CybersecurityAwarenessAssistant
{


    internal class Program
    {
        //Declerations
        private static readonly Random random = new Random();
        private static string userName;
        private static string favouriteTopic;

        //Keyword dictionary and responses
        private static readonly Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>
            {
                 {
                     "password",new List<string>
                     {
                        "Use different passwords for each account.",
                        "Enable two-factor authentication (2FA) whenever possible.",
                        "Avoid common passwords like '123456' or 'password'.",
                        "Do not share your passwords with anyone."
                     }

                 },

                  {
                     "scam",new List<string>
                     {
                    "Always verify the authenticity of messages requesting money.",
                    "Do not send money to unknown contacts.",
                    "Be skeptical of too-good-to-be-true offers.",
                    "Report suspicious activity to authorities."
                     }

                 },

                 {
                     "phishing",new List<string>
                     {
                       "Never provide sensitive info via email or unverified links.",
                        "Hover over links to verify their destination before clicking.",
                        "Verify the sender's address carefully.",
                        "Be cautious of unexpected emails requesting personal info."
                     }

                 },
                                  {
                     "privacy",new List<string>
                     {
                        "Avoid Public Wi-Fi Without a VPN.",
                        "Enable two-factor authentication (2FA) whenever possible.",
                        "Be Vigilant Against Phishing and Suspicious Links.",
                        "Be Cautious with Links, Attachments, and App Permissions."
                     }

                 }

            };

        //Dictionary for sentiments 

        public static Dictionary<string, Dictionary<string, List<string>>> SentimentResponses =
           new Dictionary<string, Dictionary<string, List<string>>>
       {
            {
                "worried", new Dictionary<string, List<string>>
                {
                    ["empathy"] = new List<string>
                    {
                        "I completely understand your concerns about online security - it's natural to feel worried.",
                        "Your anxiety about cybersecurity shows you're taking it seriously, which is actually great!",
                        "Feeling overwhelmed by online threats is normal - let's break it down into manageable steps.",
                        "It's smart to be concerned about digital security - awareness is the first step to protection."
                    },
                    ["reassurance"] = new List<string>
                    {
                        "The good news is that being informed and cautious already puts you ahead of most people!",
                        "With the right knowledge and tools, you can significantly reduce your risk exposure.",
                        "Most cyber threats can be prevented with basic security practices - you've got this!",
                        "Remember, cybersecurity is a journey, not a destination - every step makes you safer."
                    },
                    ["action"] = new List<string>
                    {
                        "Let's start with one simple step that will immediately improve your security.",
                        "I'll guide you through practical measures you can implement right away.",
                        "Focus on the basics first - we'll build your security foundation step by step."
                    }
                }
            },
            {
                "curious", new Dictionary<string, List<string>>
                {
                    ["encouragement"] = new List<string>
                    {
                        "I love your curiosity about cybersecurity - knowledge is power in the digital world!",
                        "Your eagerness to learn is exactly what makes someone security-savvy!",
                        "Great question! Your curiosity will help you stay ahead of cyber threats.",
                        "That's the kind of thinking that keeps people safe online - keep asking questions!"
                    },
                    ["engagement"] = new List<string>
                    {
                        "Let me share some fascinating insights about that topic!",
                        "There's so much to explore in cybersecurity - where would you like to dive deeper?",
                        "Your interest in learning more details shows you're thinking like a security professional!",
                        "I can tell you're really engaged with this - let's explore further!"
                    }
                }
            },
            {
                "frustrated", new Dictionary<string, List<string>>
                {
                    ["understanding"] = new List<string>
                    {
                        "I can sense your frustration - cybersecurity can feel overwhelming at times.",
                        "It's completely understandable to feel frustrated with all the security requirements.",
                        "You're not alone in feeling this way - many people find cybersecurity challenging initially.",
                        "I hear your frustration, and I'm here to make this as simple as possible for you."
                    },
                    ["simplification"] = new List<string>
                    {
                        "Let me break this down into simpler, more manageable pieces.",
                        "We'll focus on the essential basics first - no need to feel overwhelmed.",
                        "I'll explain this in straightforward terms without the technical jargon.",
                        "Let's start with the most important points and build from there."
                    }
                }
            },
            {
                "confident", new Dictionary<string, List<string>>
                {
                    ["validation"] = new List<string>
                    {
                        "I can tell you already have a good grasp of cybersecurity principles!",
                        "Your confidence in handling security matters is impressive!",
                        "You seem well-informed about these topics - that's excellent!",
                        "Your security-minded approach is exactly what I like to see!"
                    },
                    ["advancement"] = new List<string>
                    {
                        "Since you're comfortable with the basics, let's explore some advanced concepts.",
                        "You might be interested in some more sophisticated security strategies.",
                        "Given your knowledge level, here are some expert-level tips you might appreciate."
                    }
                }
            }
       };


        static void Main(string[] args)
        {
            //Play audio greeting
            PlayGreetingAudio("Cybersecuritychatbot .wav");

            //HeaderAscii Art 
            Console.Title = "Cyber-Security Assistant Chatbot";
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@"

            
 ██████╗██╗   ██╗██████╗ ███████╗██████╗       ███████╗███████╗ ██████╗██╗   ██╗██████╗ ██╗████████╗██╗   ██╗    ██████╗  ██████╗ ████████╗    
██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗      ██╔════╝██╔════╝██╔════╝██║   ██║██╔══██╗██║╚══██╔══╝╚██╗ ██╔╝    ██╔══██╗██╔═══██╗╚══██╔══╝    
██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝█████╗███████╗█████╗  ██║     ██║   ██║██████╔╝██║   ██║    ╚████╔╝     ██████╔╝██║   ██║   ██║       
██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗╚════╝╚════██║██╔══╝  ██║     ██║   ██║██╔══██╗██║   ██║     ╚██╔╝      ██╔══██╗██║   ██║   ██║       
╚██████╗   ██║   ██████╔╝███████╗██║  ██║      ███████║███████╗╚██████╗╚██████╔╝██║  ██║██║   ██║      ██║       ██████╔╝╚██████╔╝   ██║       
 ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝      ╚══════╝╚══════╝ ╚═════╝ ╚═════╝ ╚═╝  ╚═╝╚═╝   ╚═╝      ╚═╝       ╚═════╝  ╚═════╝    ╚═╝       
                                                                                                                                               
            
");

            //UI Setup and Text based greeting
            //welcome ascii art 
            Console.WriteLine(@"


         __    __     _                          
        / / /\ \ \___| | ___ ___  _ __ ___   ___ 
        \ \/  \/ / _ \ |/ __/ _ \| '_ ` _ \ / _ \
         \  /\  /  __/ | (_| (_) | | | | | |  __/
          \/  \/ \___|_|\___\___/|_| |_| |_|\___|
                                         
           
            
        ");
            Console.Title = "Cyber-Security Assistant Chatbot";
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Welcome to the CyberSecurity Assistance Chatbot!");
            Console.WriteLine("I am here to help you stay safe online what is your name.");//Prompt user name
            Console.ForegroundColor = ConsoleColor.Cyan;
            string userName = Console.ReadLine();//Read user input and save username
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Hey {userName}! How can I help you today?");
            Console.WriteLine("You can ask about security concerns such as password safety,phishing and safe browsing,or type 'exit' to quit.\n");


            while (true)//Loop to keep program running until user says exit 
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{userName}:  ");
                string userInput = Console.ReadLine()?.ToLower().Trim();//Format user input 
                //Error handling 
                if (string.IsNullOrEmpty(userInput))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a valid input.");
                    continue;
                }
                //Exit application upon user request
                if (userInput == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Goodbye! Have a nice day and stay safe online!");
                    break;
                }

                //Handle user input
                //Keyword detection 
                string response = GetKeywordResponse(userInput);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Chatbot: {response}");

                //Sentiment 
                DetectSentiment(userInput);

                //Get and remember users favourite topics
                if (favouriteTopic == null)
                {
                    Console.WriteLine("What is your favourite cybersecurity topic or deepest concern");
                    favouriteTopic = Console.ReadLine()?.ToLower().Trim();
                }
                else
                {
                    Console.WriteLine($"You told me your favourite to was {favouriteTopic} would you like to hear some interesting or informative topics about it? (yes/no)");
                    string answer = Console.ReadLine()?.ToLower().Trim();
                    if (answer == "yes")
                    {
                        response = GetKeywordResponse(favouriteTopic);
                        Console.WriteLine($"Chatbot: {response}");
                    }
                }

                
            }
        }


        //Greeting audio Method
        static void PlayGreetingAudio(string filePath)
        {
            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Greeting Audio", "Cybersecuritychatbot .wav");
                //Error handling for audio greeting
                if (File.Exists(fullPath))
                {
                    SoundPlayer player = new SoundPlayer(fullPath);
                    player.PlaySync();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: File not found at {fullPath}");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error playing audio: {ex.Message}");
            }
        }  


    //GetKeyword response method
        static string GetKeywordResponse(string userInput)
        {
                string input = userInput.ToLower();
                foreach (var keyword in keywordResponses.Keys)
                {
                    //Check for key
                    if (input.Contains(keyword))
                    {
                        return GetRandomResponseForKeyword(keyword);
                    }
                }
                return "Sorry,I didnt understand that  You can ask me about password safety,privacy, phishing, or scams.";
        }
        
        // Method to get a random response for a given keyword
        private static string GetRandomResponseForKeyword(string keyword)
        {
            //Use an array to organise random responses
            var responses = keywordResponses[keyword];
            int index = random.Next(responses.Count);
            return responses[index];
        }
        // Method to get a random response for a given sentiment
       
        //Sentiment detection method
        public static string GetSentimentResponse(string sentiment, string subcategory, string userName= "")
        {
            if (SentimentResponses.ContainsKey(sentiment) &&
                SentimentResponses[sentiment].ContainsKey(subcategory))
            {
                var responses = SentimentResponses[sentiment][subcategory];
                string response = responses[random.Next(responses.Count)];
                return string.IsNullOrEmpty(userName) ? response : response.Replace("you", userName);
            }
            return "";
        }

        static void DetectSentiment(string userInput)
        {
            // Use arrays for sentiments
            string[] worriedWords = { "worried", "concerned", "anxious" };
            string[] curiousWords = { "curious", "interested", "wondering" };
            string[] frustratedWords = { "frustrated", "overwhelmed", "confused" };
            // Check for worried sentiment
            if (worriedWords.Any(word => userInput.Contains(word)))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Chatbot: {GetSentimentResponse("worried", "empathy", userName)}");
                return;
            }
            // Check for curious sentiment
            if (curiousWords.Any(word => userInput.Contains(word)))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Chatbot: {GetSentimentResponse("curious", "encouragement", userName)}");
                return;
            }
            // Check for frustrated sentiment
            if (frustratedWords.Any(word => userInput.Contains(word)))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Chatbot: {GetSentimentResponse("frustrated", "understanding", userName)}");
                return;
            }
            // Default response for unrecognized sentiments
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Chatbot: I'm here to help! Feel free to ask about anything related to cybersecurity.");

        }

    }

}