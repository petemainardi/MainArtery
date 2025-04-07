using MainArtery.Utilities;
using Description = System.ComponentModel.DescriptionAttribute;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Sandbox
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            TestConditions();
            await TestAwaitableEvents();
        }

        /// <summary>
        /// Test basic usage of a condition set.
        /// </summary>
        public static void TestConditions()
        {
            ToggleSwitch toggle = new ToggleSwitch();
            Assert.IsFalse(toggle.State);
            toggle.Toggle();
            Assert.IsTrue(toggle.State);

            ConditionSet conditions = new ConditionSet([Description("Dummy: Always pass")]() => true);
            conditions.Add([Description("State must be true")]() => toggle.State);
            toggle.StateConditions.Add(conditions);

            toggle.Toggle();
            Assert.IsFalse(toggle.State);
            toggle.Toggle();
            Assert.IsFalse(toggle.State);

            Console.WriteLine("Passed conditions test.");
            Console.WriteLine();
        }

        /// <summary>
        /// Test adding/removing listeners while Invoke is running
        /// </summary>
        public static async Task TestAwaitableEvents()
        {
            async Task SleepLog(int sleepTime)
            {
                Console.WriteLine($"Starting delay: {sleepTime}ms vvvvv");
                await Task.Delay(sleepTime);
                Console.WriteLine($"Finished delay: {sleepTime}ms ^^^^^");
            }
            async Task Sleep1() => await SleepLog(1000);
            async Task Sleep2() => await SleepLog(2000);
            async Task Sleep3() => await SleepLog(3000);
            async Task Sleep5() => await SleepLog(5000);
            async Task Sleep7() => await SleepLog(7000);


            AwaitableEvent concurrencyTest = new AwaitableEvent();
            _ = concurrencyTest.AddNonSequentialListener(Sleep7);
            _ = concurrencyTest.AddSequentialListener(Sleep1);
            _ = concurrencyTest.AddSequentialListener(Sleep5);
            Task t1 = concurrencyTest.Invoke();
            _ = concurrencyTest.AddSequentialListener(Sleep2);
            _ = concurrencyTest.AddNonSequentialListener(Sleep3);
            _ = concurrencyTest.RemoveListener(Sleep5);
            _ = concurrencyTest.RemoveListener(Sleep7);
            _ = concurrencyTest.RemoveListener(Sleep2);
            Task t2 = concurrencyTest.Invoke();
            _ = concurrencyTest.RemoveListener(Sleep1);
            _ = concurrencyTest.RemoveListener(Sleep3);
            _ = concurrencyTest.RemoveListener(Sleep5);

            await Task.WhenAll(t1, t2);
            Console.WriteLine();
        }
    }
}
