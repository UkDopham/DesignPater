using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace ProjectDesignPatern.Exercice2
{
    /// <summary>
    /// Serie de methodes permettant de d'executer du travail a travers plusieurs nodes et de recuperer le resultat final
    /// </summary>
    class TestNodes
    {
        private string _pipeName { get; }
        private List<Node> _nodes;
        private NamedPipeServerStream _serverPipe;
        public List<Node> Nodes
        {
            get { return _nodes; }
        }

        public TestNodes(int numNodes)
        {
            // create nodes
            this._nodes = Node.CreateNodes(numNodes, "testnodes");

            // initialize pipes
            this._pipeName = "resultpipe";

            this._serverPipe = new NamedPipeServerStream(this._pipeName, PipeDirection.In, 10);
            int cpt = 0;
            this._nodes.ForEach(x => x.InitializeClientPipe(".", this._pipeName, cpt++));



        }


        public void SetTasks(ParameterizedThreadStart workToDo, int[] inputData)
        {
            int[][] inputDataSplitted = SplitInputSimple(inputData, this.Nodes.Count);
            for (int i = 0; i < this.Nodes.Count; i++)
            {
                this.Nodes[i].InitializeTask(workToDo, inputDataSplitted[i]);
            }
        }
        public void StartTasks()
        {
            this.Nodes.ForEach(x => x.Start());
        }


        public void ReadResultToTheEnd()
        {
            List<int> indexFinishedNodes = new List<int>();
            int indexeThread, sizeData;
            byte[] data;
            int result = 0;
            using (NamedPipeServerStream server = this._serverPipe)
            {
                server.WaitForConnection();
                while (indexFinishedNodes.Count < this.Nodes.Count && server.IsConnected)
                {
                    // Read header
                    sizeData = server.ReadByte();
                    indexeThread = server.ReadByte();
                    data = new byte[sizeData];

                    if (!indexFinishedNodes.Contains(indexeThread))
                        indexFinishedNodes.Add(indexeThread);

                    Console.WriteLine($"Tread {indexeThread}  : size -> {sizeData} | data -> {data} ");
                    Console.WriteLine($"result = {result}");


                    result += BitConverter.ToInt32(data);

                }

            }
        }








        private static int[][] SplitInputSimple(int[] inputData, int n)
        {
            int sizeArrayMax = inputData.Length / n;
            int index = 0;

            int[][] res = new int[n][];


            for (int i = 0; i < n; i++)
            {
                res[i] = new int[(inputData.Length - index > sizeArrayMax) ? sizeArrayMax : inputData.Length - index];
                for (int j = 0; j < res[i].Length; j++)
                {
                    res[i][j] = inputData[index];
                    index++;
                }
            }
            return res;
        }



        public void ClosePipe()
        {
            this._serverPipe.Close();
        }

        public static void Test(string s) { }
    }
}
