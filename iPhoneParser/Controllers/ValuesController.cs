using System;
using System.Threading.Tasks;
using System.Web.Http;
using iPhoneParser.Parser;
using Quartz;
using Quartz.Impl;

namespace iPhoneParser.Controllers
{
    public class ValuesController : ApiController, IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() => Get());
        }


        public void Get()
        {
            ParserWorker<string> Parser;
            Parser = new ParserWorker<string>(new EstoreParser());
            Parser.OnNewData += Parser_OnNewData;
            Parser.Settings = new EstoreSettings();
            Parser.Start();

        }


        public void Parser_OnNewData(object arg, string price)
        { 
            string Token = "310637143:AAE9Qjmrxg6JlgpgxFdA5aC0QIBYF21wuRQ";
            Method m = new Method(Token);
            m.SendMessage(price, "-1001384043986");
        }



    }

    public class TelegramScheduler
    {
        public static async void Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            var job = JobBuilder.Create<ValuesController>()
            .StoreDurably(true)
            .WithIdentity("send-telegram-message")
            .Build();

           await scheduler.AddJob(job, false);

            var trigger1 = TriggerBuilder.Create()
                                                .ForJob(job)
                                                .WithIdentity("trigger1")
                                                .WithSchedule(CronScheduleBuilder
                                                .DailyAtHourAndMinute(12, 00)
                                                .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Middle East Standard Time")))
                                                .Build();


            await scheduler.ScheduleJob(trigger1);


            var trigger2 = TriggerBuilder.Create()
                                                .ForJob(job)
                                                .WithIdentity("trigger2")
                                                .WithSchedule(CronScheduleBuilder
                                                .DailyAtHourAndMinute(23, 00)
                                                .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Middle East Standard Time")))
                                                .Build();

            await scheduler.ScheduleJob(trigger2);

           await scheduler.Start();
        }
    }
}
