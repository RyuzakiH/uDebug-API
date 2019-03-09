using System.Threading.Tasks;
using uDebug.API;

namespace uDebug_API
{
    class Program
    {
        static void Main(string[] args)
        {
            // Uncomment Any To Test

            // Synchronous Test
            //Test();

            // Asynchronous Test
            TestAsync().Wait();
        }


        private static void Test()
        {
            var client = new Client();


            // Search problems by category
            var problems = client.Search("*", SearchCategory.All);


            // Gets User Info
            var user = client.GetUser("dev-skill");
            

            // Loads a problem
            var problem = client.GetProblem(Judge.URI, 1001);

            // Loads a problem by url
            var problem1002 = client.GetProblem("https://udebug.com/URI/1002");



            var inputs = new string[3];

            // Gets input data
            inputs[0] = problem.GetInput(problem.Inputs[0]);

            // Another way to get input data
            inputs[1] = client.GetInput(problem.Inputs[1]);

            // Get input data by input id
            inputs[2] = client.GetInput(818703);

            

            var outputs = new string[8];

            // Gets the output of the problem with an input string
            outputs[0] = problem.GetOutput(inputs[0]);

            // Another way to get the output of the problem with a custom input string
            outputs[1] = problem.GetOutput("10 6");

            // Gets the output of the problem with an input
            outputs[2] = client.GetOutput(problem.Inputs[3]);

            // Another way to get the output of the problem with an input
            outputs[3] = problem.GetOutput(problem.Inputs[4]);

            // Another way to get the output of the problem with an input
            outputs[4] = client.GetOutput(problem, problem.Inputs[5]);

            // Gets the output of a problem identified by url with a custom input string
            outputs[5] = client.GetOutput("https://udebug.com/URI/1001", "200 5");

            // Gets the output of a problem identified by url with an input
            outputs[6] = client.GetOutput("https://udebug.com/URI/1001", problem.Inputs[6]);

            // Gets the output of a problem with a custom input string
            outputs[7] = client.GetOutput(Judge.URI, 1001, "400 5");
        }


        private static async Task TestAsync()
        {
            var client = new Client();


            // Search problems by category
            var problems = await client.SearchAsync("*", SearchCategory.All);


            // Gets User Info
            var user = await client.GetUserAsync("dev-skill");


            // Loads a problem
            var problem = await client.GetProblemAsync(Judge.URI, 1001);

            // Loads a problem by url
            var problem1002 = await client.GetProblemAsync("https://udebug.com/URI/1002");



            var inputs = new string[3];

            // Get input data
            inputs[0] = await problem.GetInputAsync(problem.Inputs[0]);

            // Another way to get input data
            inputs[1] = await client.GetInputAsync(problem.Inputs[1]);

            // Get input data by input id
            inputs[2] = await client.GetInputAsync(818703);



            var outputs = new string[8];

            // Gets the output of the problem with an input string
            outputs[0] = await problem.GetOutputAsync(inputs[0]);

            // Another way to get the output of the problem with a custom input string
            outputs[1] = await problem.GetOutputAsync("10 6");

            // Gets the output of the problem with an input
            outputs[2] = await client.GetOutputAsync(problem.Inputs[3]);

            // Another way to get the output of the problem with an input
            outputs[3] = await problem.GetOutputAsync(problem.Inputs[4]);

            // Another way to get the output of the problem with an input
            outputs[4] = await client.GetOutputAsync(problem, problem.Inputs[5]);

            // Gets the output of a problem identified by url with a custom input string
            outputs[5] = await client.GetOutputAsync("https://udebug.com/URI/1001", "200 5");

            // Gets the output of a problem identified by url with an input
            outputs[6] = await client.GetOutputAsync("https://udebug.com/URI/1001", problem.Inputs[6]);

            // Gets the output of a problem with a custom input string
            outputs[7] = await client.GetOutputAsync(Judge.URI, 1001, "400 5");
        }

    }
}
