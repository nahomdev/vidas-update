﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vidas;

namespace Vidas
{
    public partial class Service1 : ServiceBase
    {

        Subscriber subscriber;
        ResultSender sender;
        LabService labservice;
        ConfigFileHundler configFileHundler;
        Logger logger;
        bool doFlag;

        Thread resultSender;
        Thread listnerThread;
        Thread httpserver;
        public Service1()
        {
            InitializeComponent();
            configFileHundler = new ConfigFileHundler();

            subscriber = new Subscriber(configFileHundler.GetEndPoint());
            sender = new ResultSender();
            logger = new Logger();
            labservice = new LabService();
        }

        protected override void OnStart(string[] args)
        {

            listnerThread = new Thread(new ThreadStart(subscriber.Listen));

            sender.startServer();
              
            httpserver = new Thread(new ThreadStart(labservice.StartService));


           // resultSender.Start();
            listnerThread.Start();
            httpserver.Start();
        }

        protected override void OnStop()
        {
            //resultSender.Abort();
            listnerThread.Abort();
            httpserver.Abort();
        }
    }
}
