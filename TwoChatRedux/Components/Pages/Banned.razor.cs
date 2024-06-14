using Microsoft.AspNetCore.Components;
using TwoChatRedux.API.Models;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.Components.Pages;

public partial class Banned : ComponentBase
{
    public string ui_banString { get; set; } = "";
    public string ui_expiry { get; set; } = "";
    public string ui_phobic { get; set; } = "";
    public ChatUser currentUser { get; set; }
    
    protected override Task OnInitializedAsync()
    {
        currentUser = ChatApiClient.GetUser(ChatApiClient.GetHash(HttpContext.HttpContext.Connection.RemoteIpAddress.ToString()));
        return Task.CompletedTask;
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ui_banString = GetRandomMessage();
            ui_phobic = GetRandomPhobicMessage();
            ui_expiry = GetExpiry();
            StateHasChanged();
        }
        return Task.CompletedTask;
    }
    
    public string GetRandomMessage()
    {
        List<string> messages = new()
        {
            "WOMP WOMP WOMP",
            "we sent logs to nsa :) you're going to prison buddy",
            "we found the group chat",
            "you have been banned",
            "you will die before this ban expires",
            "sucks to SUCK",
            "justice has been served",
            "you have been convicted",
            "get fucked",
            "ur banned",
            "no more 2chat",
            "you have been found guilty",
            "you have been convicted",
        };
        Random random = new(Guid.NewGuid().GetHashCode());
        return messages[random.Next(0, messages.Count)];
    }

    public string GetExpiry()
    {
        TimeSpan expiry = currentUser.flags.banned.expiry - DateTime.Now;
        int years = expiry.Days / 365;
        int months = (expiry.Days % 365) / 30;
        int days = (expiry.Days % 365) % 30;
        int hours = expiry.Hours;
        int minutes = expiry.Minutes;
        int seconds = expiry.Seconds;

        List<string> components = new List<string>();

        if (years > 0) components.Add($"{years} year{(years > 1 ? "s" : "")}");
        if (months > 0 || (years > 0 && (days > 0 || hours > 0 || minutes > 0 || seconds > 0)))
            components.Add($"{months} month{(months > 1 ? "s" : "")}");
        if (days > 0 || ((years > 0 || months > 0) && (hours > 0 || minutes > 0 || seconds > 0)))
            components.Add($"{days} day{(days > 1 ? "s" : "")}");
        if (hours > 0 || ((years > 0 || months > 0 || days > 0) && (minutes > 0 || seconds > 0)))
            components.Add($"{hours} hour{(hours > 1 ? "s" : "")}");
        if (minutes > 0 || ((years > 0 || months > 0 || days > 0 || hours > 0) && seconds > 0))
            components.Add($"{minutes} minute{(minutes > 1 ? "s" : "")}");
        if (seconds > 0 || (years == 0 && months == 0 && days == 0 && hours == 0 && minutes == 0))
            components.Add($"{seconds} second{(seconds > 1 ? "s" : "")}");

        string expiryString = string.Join(", ", components);
        if (components.Count > 1)
        {
            int lastComma = expiryString.LastIndexOf(",");
            if (lastComma != -1)
            {
                expiryString = expiryString.Substring(0, lastComma) + " and" + expiryString.Substring(lastComma + 1);
            }
        }

        return expiryString;
    }

    public string GetRandomPhobicMessage()
    {
        List<string> messages = new()
        {
            "racist",
            "homophobic",
            "transphobic",
            "sexist",
            "bigoted",
            "ableist",
            "xenophobic",
            "islamophobic",
            "anti-semitic",
            "misogynistic",
            "colorist",
            "nationalist",
        };
        Random random = new(Guid.NewGuid().GetHashCode());
        return messages[random.Next(0, messages.Count)];
    }
}