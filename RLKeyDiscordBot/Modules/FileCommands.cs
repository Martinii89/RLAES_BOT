using Discord.Commands;
using Discord;
using System;
using System.Threading.Tasks;
using RLKeyDiscordBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RLKeyDiscordBot.Modules
{
    public class FileCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IConfigurationRoot _config;
        private readonly Fileservice _fileService;

        public FileCommands(IServiceProvider services)
        {
            _config = services.GetRequiredService<IConfigurationRoot>();
            _fileService = services.GetRequiredService<Fileservice>();
        }

        [Command("decryptor", RunMode = RunMode.Async)]
        [Summary("Sends the decyptor.")]
        public async Task SendDecryptor(IUser user = null) =>
            await _fileService.SendFile(_config["DecryptorPath"], user ?? Context.User);

        [Command("ueedlls", RunMode = RunMode.Async)]
        [Summary("Sends the dlls needed for Unreal Explorer.")]
        public async Task SendUEEDlls(IUser user = null) =>
            await _fileService.SendFile(_config["UEEDllsPath"], user ?? Context.User);

        [Command("uee", RunMode = RunMode.Async)]
        [Summary("Sends UnrealExplorer with the dlls needed to open up RL upks.")]
        public async Task SendUEE(IUser user = null) =>
            await _fileService.SendFile(_config["UEEPath"], user ?? Context.User);

        [Command("umodel", RunMode = RunMode.Async)]
        [Summary("Sends the umodel version compatible with rocket league.")]
        public async Task SendUmodelZip(IUser user = null) =>
            await _fileService.SendFile(_config["UmodelPath"], user ?? Context.User);

        [Command("assetextractor", RunMode = RunMode.Async)]
        [Summary("Sends the Asset Extractor.")]
        public async Task SendAssetExtractorZip(IUser user = null) =>
            await _fileService.SendFile(_config["AssetExtractorPath"], user ?? Context.User);

        [Command("keys", RunMode = RunMode.Async)]
        [Summary("Sends a txt file with the current decrypted keys.")]
        public async Task SendKeys(IUser user = null) => 
            await _fileService.SendFile(_config["KeysPath"], user ?? Context.User);
    }
}