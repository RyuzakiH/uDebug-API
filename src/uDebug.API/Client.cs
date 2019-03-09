using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using uDebug.API.Models;

namespace uDebug.API
{
    public class Client
    {
        private CookieContainer cookies;

        private Search search;
        private Problem problem;

        public HttpClient HttpClient { get; private set; }


        public Client()
        {
            CreateHttpClient();

            search = new Search(this);
            problem = new Problem(this);
        }




        
        public List<Problem> Search(string term, SearchCategory category)
        {
            return search.SearchProblems(term, category);
        }

        public async Task<List<Problem>> SearchAsync(string term, SearchCategory category)
        {
            return await search.SearchProblemsAsync(term, category);
        }


        public User GetUser(string username)
        {
            var user = new User(this);
            user.Load(username);
            return user;
        }

        public async Task<User> GetUserAsync(string username)
        {
            var user = new User(this);
            await user.LoadAsync(username);
            return user;
        }



        public Problem GetProblem(Judge judge, int problemId)
        {
            var problem = new Problem(this);
            problem.Load(judge, problemId);
            return problem;
        }

        public async Task<Problem> GetProblemAsync(Judge judge, int problemId)
        {
            var problem = new Problem(this);
            await problem.LoadAsync(judge, problemId);
            return problem;
        }

        public Problem GetProblem(string url)
        {
            var problem = new Problem(this);
            problem.Load(url);
            return problem;
        }

        public async Task<Problem> GetProblemAsync(string url)
        {
            var problem = new Problem(this);
            await problem.LoadAsync(url);
            return problem;
        }



        public string GetInput(Input input)
        {
            return input.Data = GetInput(input.Id);
        }

        public async Task<string> GetInputAsync(Input input)
        {
            return input.Data = await GetInputAsync(input.Id);
        }

        public string GetInput(int inputId)
        {
            return problem.GetInput(inputId);
        }

        public async Task<string> GetInputAsync(int inputId)
        {
            return await problem.GetInputAsync(inputId);
        }



        public string GetOutput(Input input)
        {
            return input.Problem.GetOutput(input);
        }

        public async Task<string> GetOutputAsync(Input input)
        {
            return await input.Problem.GetOutputAsync(input);
        }

        public string GetOutput(Problem problem, Input input)
        {
            return problem.GetOutput(input);
        }

        public async Task<string> GetOutputAsync(Problem problem, Input input)
        {
            return await problem.GetOutputAsync(input);
        }

        public string GetOutput(string problemUrl, Input input)
        {
            var problem = new Problem(this);
            problem.Load(problemUrl);
            return problem.GetOutput(input);
        }

        public async Task<string> GetOutputAsync(string problemUrl, Input input)
        {
            var problem = new Problem(this);
            await problem.LoadAsync(problemUrl);
            return await problem.GetOutputAsync(input);
        }

        public string GetOutput(string problemUrl, string input)
        {
            var problem = new Problem(this);
            problem.Load(problemUrl);
            return problem.GetOutput(input);
        }

        public async Task<string> GetOutputAsync(string problemUrl, string input)
        {
            var problem = new Problem(this);
            await problem.LoadAsync(problemUrl);
            return await problem.GetOutputAsync(input);
        }

        public string GetOutput(Judge judge, int problemId, string input)
        {
            var problem = new Problem(this);
            problem.Load(judge, problemId);
            return problem.GetOutput(input);
        }

        public async Task<string> GetOutputAsync(Judge judge, int problemId, string input)
        {
            var problem = new Problem(this);
            await problem.LoadAsync(judge, problemId);
            return await problem.GetOutputAsync(input);
        }



        private void CreateHttpClient()
        {
            cookies = new CookieContainer();

            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = cookies,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                //AllowAutoRedirect = true
            };

            HttpClient = new HttpClient(handler);

            HttpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            HttpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en");
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.109 Safari/537.36");
            //client.DefaultRequestHeaders.Add("Host", "www.udebug.org");
            //client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            HttpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        }

    }
}
