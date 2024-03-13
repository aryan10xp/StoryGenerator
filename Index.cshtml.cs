using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoryGenerator.Models;
using Azure.AI.OpenAI;

namespace StoryGenerator.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public StoryDetails Details { get; set; } = new();

        public string StoryContent { get; set; }

        public void OnGet()
        {

        }
        public void OnPost()
        {
            var key = " 567bf9aa8caf4b4eb3b3c3b42f8fc745";
            var endpoint = "https://azuredocscopilotdev.openai.azure.com/";
            

            OpenAIClient aiClient = new(new Uri(endpoint), new Azure.AzureKeyCredential(key));

            var prompt = $"Write me a story called {Details.Title} in a {Details.Tone} tone about a"
                + $"{Details.Creature} named {Details.Name} who lives in a {Details.Environment} Use at most 100 words.";

            ChatCompletionsOptions chatOptions = new ChatCompletionsOptions()
            {
                DeploymentName = "gpt-4-32k",
                Messages =
                {
                    new ChatRequestSystemMessage("You are a helpful assistant."),
                    new ChatRequestUserMessage(prompt)
                }
          

            };

            ChatCompletions responseCompletion = aiClient.GetChatCompletions(chatOptions);

            StoryContent = responseCompletion.Choices[0].Message.Content;


        }
    }
}