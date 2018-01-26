using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Linq;
using Identity.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Identity.API.Data
{


    public class ApplicationDbContextSeed
    {
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();

        public async Task SeedAsync(ApplicationDbContext context,IHostingEnvironment env,
            ILogger<ApplicationDbContextSeed> logger, IOptions<AppSettings> settings,int? retry = 0)
        {
            int retryForAvaiability = retry.Value;

            try
            {
                var useCustomizationData = settings.Value.UseCustomizationData;
                var contentRootPath = env.ContentRootPath;
                var webroot = env.WebRootPath;

                if (!context.Users.Any())
                {
                    context.Users.AddRange(useCustomizationData
                        ? GetUsersFromFile(contentRootPath, logger)
                        : GetDefaultUser(logger));

                    await context.SaveChangesAsync();
                }

                if (useCustomizationData)
                {
                    GetPreconfiguredImages(contentRootPath, webroot, logger);
                }
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;
                    
                    logger.LogError(ex.Message,$"There is an error migrating data for ApplicationDbContext");

                    await SeedAsync(context,env,logger,settings, retryForAvaiability);
                }
            }
        }

        private IEnumerable<ApplicationUser> GetUsersFromFile(string contentRootPath, ILogger logger)
        {
            string fileName = Path.Combine(contentRootPath, "Setup", "Users.json");

            if (!File.Exists(fileName))
            {
                return GetDefaultUser(logger);
            }

			string raw = File.ReadAllTextAsync(fileName).Result;
            return GetUsersFromJson(raw, logger);
        }

		private IEnumerable<ApplicationUser> GetDefaultUser(ILogger logger)
		{
			string json =
				"[" +
				"  {" +
				"    \"CardHolderName\": \"DemoUser\"," +
				"    \"CardNumber\": 4012888888881881," +
				"    \"CardType\": 1," +
				"    \"City\": \"Redmond\"," +
				"    \"Country\": \"U.S.\"," +
				"    \"Email\": \"demouser@microsoft.com\"," +
				"    \"Expiration\": \"12/20\"," +
				"    \"LastName\": \"DemoLastName\"," +
				"    \"Name\": \"DemoUser\"," +
				"    \"PhoneNumber\": 1234567890," +
				"    \"UserName\": \"demouser@microsoft.com\"," +
				"    \"ZipCode\": 98052," +
				"    \"State\": \"WA\"," +
				"    \"Street\": \"15703 NE 61st Ct\"," +
				"    \"SecurityNumber\": 535," +
				"    \"NormalizedEmail\": \"DEMOUSER@MICROSOFT.COM\"," +
				"    \"NormalizedUserName\": \"DEMOUSER@MICROSOFT.COM\"," +
				"    \"Password\": \"Pass@word1\"" +
				"  }" +
				"]";

			return GetUsersFromJson(json, logger);
		}

		private IEnumerable<ApplicationUser> GetUsersFromJson(string json, ILogger logger)
		{
			dynamic items = JArray.Parse(json);
			List<ApplicationUser> users = new List<ApplicationUser>();

			foreach (dynamic item in items)
			{
				users.Add(CreateApplicationUser(item));
			}
			return users;
		}

		private ApplicationUser CreateApplicationUser(dynamic u)
		{
			var user = new ApplicationUser();

			user.CardHolderName = u.CardHolderName;
			user.CardNumber = u.CardNumber;
			user.CardType = u.CardType;
			user.City = u.City;
			user.Country = u.Country;
			user.Email = u.Email;
			user.Expiration = u.Expiration;
			user.Id = Guid.NewGuid().ToString();
			user.LastName = u.LastName;
			user.Name = u.Name;
			user.PhoneNumber = u.PhoneNumber;
			user.UserName = u.UserName;
			user.ZipCode = u.ZipCode;
			user.State = u.State;
			user.Street = u.Street;
			user.SecurityNumber = u.SecurityNumber;
			user.NormalizedEmail = u.NormalizedEmail;
			user.NormalizedUserName = u.NormalizedUserName;
			user.SecurityStamp = Guid.NewGuid().ToString("D");
			user.PasswordHash = u.Password; // Note: This is the password
			

			user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);

			return user;
		}

        static void GetPreconfiguredImages(string contentRootPath, string webroot, ILogger logger)
        {
            try
            {
                string imagesZipFile = Path.Combine(contentRootPath, "Setup", "images.zip");
                if (!File.Exists(imagesZipFile))
                {
                    logger.LogError($" zip file '{imagesZipFile}' does not exists.");
                    return;
                }

                string imagePath = Path.Combine(webroot, "images");
                string[] imageFiles = Directory.GetFiles(imagePath).Select(file => Path.GetFileName(file)).ToArray();

                using (ZipArchive zip = ZipFile.Open(imagesZipFile, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        if (imageFiles.Contains(entry.Name))
                        {
                            string destinationFilename = Path.Combine(imagePath, entry.Name);
                            if (File.Exists(destinationFilename))
                            {
                                File.Delete(destinationFilename);
                            }
                            entry.ExtractToFile(destinationFilename);
                        }
                        else
                        {
                            logger.LogWarning($"Skip file '{entry.Name}' in zipfile '{imagesZipFile}'");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception in method GetPreconfiguredImages WebMVC. Exception Message={ex.Message}");
            }
        }
    }
}
