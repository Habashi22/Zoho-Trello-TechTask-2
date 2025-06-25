//using System;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

//class Program
//{
//    static string zohoAccessToken = "1000.b97a1037fa413603aca8e59a9ae8d8fb.7f9fba043b2eb95bcf6dfc7c6806d33d";
//    static string trelloKey = "aef120e9d76700a7dd8f6398f6c75f77";
//    static string trelloToken = "a27b94f2cd86354be9d7d0e84887fcb1f1400f0fd8e932c875d31f1e5e92d1a1";

//    static async Task Main(string[] args)
//    {
//        Console.WriteLine("🔄 Starting Zoho → Trello Integration...");

//        var deals = await GetZohoDeals();

//        Console.WriteLine($"🔍 Fetched {deals.Count} deals from Zoho CRM.");

//        foreach (var deal in deals)
//        {
//            string dealStage = deal.Value<string>("Stage");
//            string dealType = deal.Value<string>("Type"); // Adjusted field name
//            string projectBoardId = deal.Value<string>("Project_Board_ID__c");
//            string dealId = deal.Value<string>("id");
//            string dealName = deal.Value<string>("Deal_Name");

//            Console.WriteLine($"Checking deal: {dealName} | Stage: {dealStage}, Type: {dealType}, Board ID: {projectBoardId}");

//            if (dealStage == "Project Kickoff" &&
//                dealType == "New Business" &&     // Match your Zoho picklist value
//                string.IsNullOrEmpty(projectBoardId))
//            {
//                Console.WriteLine($"✅ Creating Trello board for Deal: {dealName}");

//                string boardId = await CreateTrelloBoard(dealName);

//                if (!string.IsNullOrEmpty(boardId))
//                {
//                    Console.WriteLine($"🔁 Updating Zoho Deal {dealId} with Trello Board ID: {boardId}");
//                    await UpdateZohoDeal(dealId, boardId);
//                }
//            }
//        }

//        Console.WriteLine("✅ Integration complete.");
//    }

//    static async Task<JArray> GetZohoDeals()
//    {
//        using var client = new HttpClient();
//        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", zohoAccessToken);

//        string url = "https://www.zohoapis.com/crm/v2/Deals";

//        var response = await client.GetAsync(url);
//        response.EnsureSuccessStatusCode();

//        string json = await response.Content.ReadAsStringAsync();
//        var obj = JObject.Parse(json);
//        return (JArray)obj["data"];
//    }

//    static async Task<string> CreateTrelloBoard(string dealName)
//    {
//        using var client = new HttpClient();

//        var boardResponse = await client.PostAsync(
//            $"https://api.trello.com/1/boards/?name=Project:%20{Uri.EscapeDataString(dealName)}&key={trelloKey}&token={trelloToken}",
//            null);

//        if (!boardResponse.IsSuccessStatusCode)
//        {
//            Console.WriteLine("❌ Failed to create Trello board.");
//            return null;
//        }

//        var boardJson = await boardResponse.Content.ReadAsStringAsync();
//        var boardObj = JObject.Parse(boardJson);
//        string boardId = boardObj["id"].ToString();

//        // Create lists
//        string[] lists = { "To Do", "In Progress", "Done" };
//        foreach (var listName in lists)
//        {
//            await client.PostAsync(
//                $"https://api.trello.com/1/lists?name={Uri.EscapeDataString(listName)}&idBoard={boardId}&key={trelloKey}&token={trelloToken}",
//                null);
//        }

//        // Get the 'To Do' list ID
//        var listsResponse = await client.GetAsync(
//            $"https://api.trello.com/1/boards/{boardId}/lists?key={trelloKey}&token={trelloToken}");
//        var listsJson = await listsResponse.Content.ReadAsStringAsync();
//        var listsArray = JArray.Parse(listsJson);

//        string toDoListId = null;
//        foreach (var list in listsArray)
//        {
//            if (list["name"].ToString() == "To Do")
//            {
//                toDoListId = list["id"].ToString();
//                break;
//            }
//        }

//        if (toDoListId != null)
//        {
//            string[] cards = { "Kickoff Meeting Scheduled", "Requirements Gathering", "System Setup" };
//            foreach (var cardName in cards)
//            {
//                await client.PostAsync(
//                    $"https://api.trello.com/1/cards?name={Uri.EscapeDataString(cardName)}&idList={toDoListId}&key={trelloKey}&token={trelloToken}",
//                    null);
//            }
//        }

//        return boardId;
//    }

//    static async Task UpdateZohoDeal(string dealId, string boardId)
//    {
//        using var client = new HttpClient();
//        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", zohoAccessToken);

//        var body = new
//        {
//            data = new[]
//            {
//                new
//                {
//                    id = dealId,
//                    Project_Board_ID__c = boardId
//                }
//            }
//        };

//        var jsonBody = JsonConvert.SerializeObject(body);
//        var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

//        var url = $"https://www.zohoapis.com/crm/v2/Deals";

//        var response = await client.PutAsync(url, content);

//        if (!response.IsSuccessStatusCode)
//        {
//            Console.WriteLine($"❌ Failed to update deal {dealId}: {response.StatusCode}");
//        }
//        else
//        {
//            Console.WriteLine($"✅ Deal {dealId} updated successfully in Zoho.");
//        }
//    }
//}


using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program
{
    static string zohoAccessToken = "1000.b97a1037fa413603aca8e59a9ae8d8fb.7f9fba043b2eb95bcf6dfc7c6806d33d";
    //static string trelloKey = "aef120e9d76700a7dd8f6398f6c75f77";
    //static string trelloToken = "a27b94f2cd86354be9d7d0e84887fcb1f1400f0fd8e932c875d31f1e5e92d1a1";
    static string trelloKey = "0860ed15666b34ca98f21d209f30a6a5";
    static string trelloToken = "ATTA98da0ff36eba251809da3921c39ee986f876bfad169796df8148de66509a93e06678166C";


    static async Task Main(string[] args)
    {
        Console.WriteLine("🔄 Starting Zoho → Trello Integration...");

        var deals = await GetZohoDeals();

        Console.WriteLine($"🔍 Fetched {deals.Count} deals from Zoho CRM.");

        foreach (var deal in deals)
        {
            string dealStage = deal.Value<string>("Stage");
            string dealType = deal.Value<string>("Type"); // Adjust if field name differs
            string projectBoardId = deal.Value<string>("Project_Board_ID__c");
            string dealId = deal.Value<string>("id");
            string dealName = deal.Value<string>("Deal_Name");

            Console.WriteLine($"Checking deal: {dealName} | Stage: {dealStage}, Type: {dealType}, Board ID: {projectBoardId}");

            // Check your real stage and type values here
            if (dealStage == "Needs Analysis" &&
                dealType == "New Business" &&
                string.IsNullOrEmpty(projectBoardId))
            {
                Console.WriteLine($"✅ Creating Trello board for Deal: {dealName}");

                string boardId = await CreateTrelloBoard(dealName);

                if (!string.IsNullOrEmpty(boardId))
                {
                    Console.WriteLine($"🔁 Updating Zoho Deal {dealId} with Trello Board ID: {boardId}");
                    await UpdateZohoDeal(dealId, boardId);
                }
            }
        }

        Console.WriteLine("✅ Integration complete.");
    }

    static async Task<JArray> GetZohoDeals()
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", zohoAccessToken);

        string url = "https://www.zohoapis.com/crm/v2/Deals";

        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        var obj = JObject.Parse(json);
        return (JArray)obj["data"];
    }

    static async Task<string> CreateTrelloBoard(string dealName)
    {
        using var client = new HttpClient();

        var boardResponse = await client.PostAsync(
            $"https://api.trello.com/1/boards/?name=Project:%20{Uri.EscapeDataString(dealName)}&key={trelloKey}&token={trelloToken}",
            new StringContent(""));

        if (!boardResponse.IsSuccessStatusCode)
        {
            Console.WriteLine("❌ Failed to create Trello board.");
            return null;
        }

        var boardJson = await boardResponse.Content.ReadAsStringAsync();
        var boardObj = JObject.Parse(boardJson);
        string boardId = boardObj["id"].ToString();

        // Create lists on the new board
        string[] lists = { "To Do", "In Progress", "Done" };
        foreach (var listName in lists)
        {
            var listResponse = await client.PostAsync(
                $"https://api.trello.com/1/lists?name={Uri.EscapeDataString(listName)}&idBoard={boardId}&key={trelloKey}&token={trelloToken}",
                new StringContent(""));

            if (!listResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ Failed to create Trello list '{listName}'.");
            }
        }

        // Get the 'To Do' list ID to add cards
        var listsResponse = await client.GetAsync(
            $"https://api.trello.com/1/boards/{boardId}/lists?key={trelloKey}&token={trelloToken}");
        var listsJson = await listsResponse.Content.ReadAsStringAsync();
        var listsArray = JArray.Parse(listsJson);

        string toDoListId = null;
        foreach (var list in listsArray)
        {
            if (list["name"].ToString() == "To Do")
            {
                toDoListId = list["id"].ToString();
                break;
            }
        }

        if (toDoListId != null)
        {
            string[] cards = { "Kickoff Meeting Scheduled", "Requirements Gathering", "System Setup" };
            foreach (var cardName in cards)
            {
                await client.PostAsync(
                    $"https://api.trello.com/1/cards?name={Uri.EscapeDataString(cardName)}&idList={toDoListId}&key={trelloKey}&token={trelloToken}",
                    new StringContent(""));
            }
        }

        return boardId;
    }

    static async Task UpdateZohoDeal(string dealId, string boardId)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", zohoAccessToken);

        var body = new
        {
            data = new[]
            {
                new
                {
                    id = dealId,
                    Project_Board_ID__c = boardId
                }
            }
        };

        var jsonBody = JsonConvert.SerializeObject(body);
        var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        // Correct update URL with dealId appended
        var url = $"https://www.zohoapis.com/crm/v2/Deals/{dealId}";

        var response = await client.PutAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"❌ Failed to update deal {dealId}: {response.StatusCode}");
        }
        else
        {
            Console.WriteLine($"✅ Deal {dealId} updated successfully in Zoho.");
        }
    }
}
