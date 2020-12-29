using ProjectDesignPatern.Exercice2.IPC;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace ProjectDesignPatern.Exercice2
{
    class Node
    {
        public string name { get; }
        public bool isPipe { get; }
        public bool hasFinished { get; }
        public NamedPipeClientStream _clientPipe;

        public Thread _worker;
        private Thread Worker
        {
            get { return this._worker; }
            set { this._worker = value; }
        }

        private int[] _workToDo;
        public int[] WorkToDo
        {
            get { return this._workToDo; }
            set { this._workToDo = value; }
        }
        ParameterizedThreadStart threadStart { get; set; }


        public static List<Node> CreateNodes(int numNodes, string nameNodes)
        {
            List<Node> listN = new List<Node>();

            for (int i = 0; i < numNodes; i++)
            {
                listN.Add(new Node($"{nameNodes}{i}"));
            }
            return listN;
        }


        public Node(string name)
        {
            this.name = name;
            this.hasFinished = true;
            this.isPipe = true;
        }

        public void InitializeClientPipe(string serverName, string pipeName, int i)
        {
            this._clientPipe = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut);

        }
        public void ReturnResult()
        {
            throw new NotImplementedException();
        }
        public void InitializeTask(ParameterizedThreadStart threadStart, int[] inputData)
        {
            this.threadStart = threadStart;   
            this.WorkToDo = inputData;
        }

        public void Start()
        {
            this.Worker = new Thread(this.threadStart);
            this.Worker.Start((object)(new object[] { WorkToDo, this._clientPipe }));


        }



    }

}
