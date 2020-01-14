using System;
using System.Threading;
using System.Threading.Tasks;

namespace Morromation
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Write("Enter timespan in seconds for loop interval: ");
            var duration = Convert.ToInt32(Console.ReadLine());

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            var mouseLoopTask = new Task(async () =>
            {

                var timeSpan = new TimeSpan(0,0, duration);
                var mousePosition = MouseOps.GetCursorPosition();
                await ClickAtPositionLoopAsync(mousePosition.X, mousePosition.Y, timeSpan, token);
            });

            Console.WriteLine("When mouse is at desired position enter 'S' to start, and 'Q' to quit.");
            while (!token.IsCancellationRequested)
            {

                var keyInput = Console.ReadKey();
                switch (keyInput.KeyChar.ToString().ToLower())
                {
                    case "q":
                        cancellationTokenSource.Cancel();
                        break;
                    case "s":
                        mouseLoopTask.Start();
                        break;
                }
            }

        }

        private static async Task ClickAtPositionLoopAsync(int x, int y, TimeSpan waitTime, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                ClickAtPosition(x, y);
                await Task.Delay(waitTime, token);

            }
        }

        private static void ClickAtPosition(int x, int y)
        {
            MouseOps.SetCursorPosition(x, y);
            MouseOps.MouseEvent(MouseOps.MouseEventFlags.LeftDown);
            MouseOps.MouseEvent(MouseOps.MouseEventFlags.LeftUp);
        }

    }
}
