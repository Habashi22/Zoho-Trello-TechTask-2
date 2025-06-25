# 🔁 Zoho CRM → Trello Integration

This project automates the creation of Trello boards for Zoho CRM deals when certain conditions are met. It uses Zoho and Trello public APIs without relying on built-in automation tools.

## ✅ Features

- Authenticates with **Zoho CRM** using OAuth2 access token
- Authenticates with **Trello** using API key & token
- Polls Zoho CRM for deals
- Creates a **new Trello board** for deals that match:
  - Stage = `Needs Analysis`
  - Type = `New Business`
  - `Project_Board_ID__c` field is empty
- Adds default lists and cards to the board
- Updates Zoho CRM deal with the new board ID

---

## 🔧 Setup

### 1. Prerequisites

- [.NET 6 or later](https://dotnet.microsoft.com/)
- Zoho CRM account (with OAuth credentials + access token)
- Trello account (API key + token)

### 2. Fill in Your Credentials

In `Program.cs`, update the following lines:

```csharp
static string zohoAccessToken = "YOUR_ZOHO_ACCESS_TOKEN";
static string trelloKey = "YOUR_TRELLO_API_KEY";
static string trelloToken = "YOUR_TRELLO_TOKEN";

📋 Example Output
yaml
Copy
Edit
🔄 Starting Zoho → Trello Integration...
🔍 Fetched 11 deals from Zoho CRM.
Checking deal: Integration Test Deal | Stage: Needs Analysis, Type: New Business, Board ID:
✅ Creating Trello board for Deal: Integration Test Deal
🔁 Updating Zoho Deal 6866494000000602004 with Trello Board ID: 685c38c671ae3c75f0ab7201
✅ Deal updated successfully in Zoho.
...
✅ Integration complete.

How It Works
Deals are fetched from Zoho CRM using /crm/v2/Deals

Each deal is checked:

Is Stage = "Needs Analysis"?

Is Type = "New Business"?

Is Project_Board_ID__c empty?

If all match:

Create a Trello board: Project: {Deal Name}

Add 3 Lists:

To Do

In Progress

Done

Add 3 cards under To Do

Kickoff Meeting Scheduled

Requirements Gathering

System Setup

Save board ID back to Zoho CRM
