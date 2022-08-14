using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Practica2
{
    public partial class Form1 : Form
    {

        

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            //Obtener();
            timer1.Enabled = true;
        }



        public async Task<string> OBCPU(string P)
        {

            var Proceso = new Process[0];
           
                Proceso = Process.GetProcessesByName(DGV.SelectedRows[0].Cells[0].Value.ToString());
            
            var process = "";
            Parallel.ForEach(Proceso, item => process = item.ProcessName);

            var cpu = new PerformanceCounter("Process", "% Processor Time", process);
            cpu.NextValue();
            await Task.Delay(1000);
            Double porciento = (double)cpu.NextValue() / 10;
            string number = porciento.ToString();
            number = Decimal.Parse(porciento.ToString()).ToString("#.##") + "%";
            if(porciento > 0 && porciento.ToString().Contains("0"))
            {
                number = "0" + number;
                return number;
            }
            else
            {
                number = "0%";
            }
            return number;

        }

        public async Task<string> OBRAM(string P)
        {
            var proceso = new Process[0];
            proceso = Process.GetProcessesByName(DGV.SelectedRows[0].Cells[0].Value.ToString());

            var process = new long();
            Parallel.ForEach(proceso, item => process = item.PeakWorkingSet64/104857);

            return process.ToString();

        }

        public async Task<string> OBID(string P)
        {
            var proceso = new Process[0];
            proceso = Process.GetProcessesByName(DGV.SelectedRows[0].Cells[0].Value.ToString());

            var process = new long();
            Parallel.ForEach(proceso, item => process = item.Id);

            return process.ToString();

        }


        private void ProcessList()
        {

            int id = 1;

            Process[] PP = new Process[0];

            foreach (Process p in Process.GetProcesses())
            {


                decimal number = (decimal)p.WorkingSet64;
                while (Math.Round(number / 1024) >= 1)
                {
                    number = number / 1024;
                }

                number = number / 1024;



                label1.Text = (number * 100 / DGV.Rows.Count).ToString("0. ## '%'");
                String porcentaje;
                porcentaje = label1.Text;



                DGV.Rows.Add(/*id + ":" + */p.ProcessName, p.Id, p.SessionId, porcentaje); // nombre del proceso
                id = id + 1;

                //Process[] remoteByName = Process.GetProcessesByName(DGV.SelectedRows[0].Cells[0].Value.ToString());

                //foreach (var pp in remoteByName)
                //{
                //    listView1.Items.Add($"Process found with ID: {pp.Id}");
                //    Thread.Sleep(3000);
                //}


            }



            PCount.Text = "Procesos Actuales: " + DGV.Rows.Count.ToString();    //  cant de procesos   

        }



        private void Form1_Load(object sender, EventArgs e)
        {
            ProcessList();
        }
        public async Task Obtener()
        {
            Process[] TP = Process.GetProcesses();
            var conteo = 0;
            try
            {
                foreach (Process procesos in Process.GetProcesses())
                {
                    //        {
                    Parallel.ForEach(TP, procesos =>
                {

                    DGV.Rows.Add(procesos.ProcessName);
                    conteo++;
                });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }



        private void DGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
            
                //String Nombre = this.DGV.SelectedRows[0].Cells[0].Value.ToString();
                //int PID = Convert.ToInt32(this.DGV.SelectedRows[0].Cells[1].Value);
                
                //decimal RAM = Convert.ToDecimal(this.DGV.SelectedRows[0].Cells[3].Value);

                
                //MessageBox.Show(
                //    listView1.Items.Add($""+ $"Process ID: {Value}")
                //);
                //listView1.Clear();
                //listView1.Items.Add($"Proceso: {Nombre}, \n\nPID:{PID} , \n CPU: {CPU} ,\n Ram:{RAM}");
           

        }

        private async void DGV_SelectionChanged(object sender, EventArgs e)
        {
            var ID = await OBID("Proceso");
            var CPU = await OBCPU("Proceso");
            var RAM = await OBRAM("Proceso");

            listView1.Items.Add($"" +$"PID:{ID}"+ $"CPU: {CPU}\r\n"+$"RAM: {RAM}");

        }

        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void PCount_Click(object sender, EventArgs e)
        {

        }
    }
}
