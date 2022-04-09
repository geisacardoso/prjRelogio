using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjRelogio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Graphics g;

        int hora;
        int min;
        int seg;

        int xseg, yseg;
        int xmin, ymin;
        int xhora, yhora;

        string caminho;
        string arquivo;

        Bitmap desenho;

        private void Form1_Load(object sender, EventArgs e)
        {

            caminho = Environment.CurrentDirectory;
            arquivo = caminho + @"\fundo.png";
            desenho = new Bitmap(pbImagem.Width, pbImagem.Height);

            g = Graphics.FromImage(desenho);

            if (File.Exists(arquivo))
            {
                pbImagem.Image = Image.FromFile(arquivo);
            }
            else
            {
                openFileDialog1.ShowDialog();
                File.Copy(openFileDialog1.FileName, arquivo);
                pbImagem.Image = Image.FromFile(arquivo);
           }

            pulso.Enabled = true;

        }

        private void pulso_Tick(object sender, EventArgs e)
        {
            hora = DateTime.Now.Hour;
            min = DateTime.Now.Minute;
            seg = DateTime.Now.Second;
            desenharSegundo();
            desenharMinuto();
            desenharHora();
            desenharCentro();
            pbImagem.CreateGraphics().DrawImage(desenho,
                new Rectangle(0,0,
                pbImagem.Width,pbImagem.Height)
                );

            relogioDigital();
        }

        private void relogioDigital()
        {
            lbHorario.Text = DateTime.Now.ToLongTimeString() +
                "\n" + DateTime.Now.ToLongDateString();
        }

        private void desenharCentro()
        {
            int cx = pbImagem.Width / 2;
            int cy = pbImagem.Height / 2;
            SolidBrush balde = new SolidBrush(Color.Blue);
            g.FillEllipse(balde, cx - 10, cy - 10, 20, 20);
        }

        private void desenharHora()
        {

            int cx = pbImagem.Width / 2;
            int cy = pbImagem.Height / 2;
            double raio = 150;

            Pen caneta = new Pen(Color.Transparent, 6);
            caneta.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            caneta.StartCap = System.Drawing.Drawing2D.LineCap.Round;

            double intervalo = 360 / 12;

            if (hora > 12) hora = hora - 12;

            double angulo = -90 + intervalo * (hora);
        
            g.DrawLine(caneta, cx, cy, cx + xhora, cy + yhora);
            double rad = GrauToRadiano(angulo);

            xhora = (int)(raio * Math.Cos(rad));
            yhora = (int)(raio * Math.Sin(rad));

            caneta.Color = Color.Yellow;
            g.DrawLine(caneta, cx, cy, cx + xhora, cy + yhora);
        }

        private void desenharMinuto()
        {
            int cx = pbImagem.Width / 2;
            int cy = pbImagem.Height / 2;
            double raio = 160;
            Pen caneta = new Pen(Color.Transparent, 6);
            caneta.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            caneta.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            double intervalo = 360 / 60;
            double angulo = -90 + intervalo * min;
            g.DrawLine(caneta, cx, cy, cx + xmin, cy + ymin);
            double rad = GrauToRadiano(angulo);

            xmin = (int)(raio * Math.Cos(rad));
            ymin = (int)(raio * Math.Sin(rad));

            caneta.Color = Color.Blue;
            g.DrawLine(caneta, cx, cy, cx + xmin, cy + ymin);
        }

        private void desenharSegundo()
        {
            int cx = pbImagem.Width / 2;
            int cy = pbImagem.Height / 2;
            double raio = pbImagem.Width/2;
            Pen caneta = new Pen(Color.Transparent, 6);
            caneta.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            caneta.StartCap = System.Drawing.Drawing2D.LineCap.Round;

            g.DrawImage(Image.FromFile(arquivo),new Rectangle(
                0,0,pbImagem.Width,pbImagem.Height) );

            double intervalo = 360 / 60;
            double angulo = -90 + intervalo * seg;
            g.DrawLine(caneta, cx, cy, cx + xseg, cy + yseg);
            double rad = GrauToRadiano(angulo);
            xseg = (int)(raio * Math.Cos(rad));
            yseg = (int)(raio * Math.Sin(rad));

            caneta.Color = Color.DarkBlue;
            g.DrawLine(caneta, cx, cy, cx + xseg, cy + yseg);
        }

        private double GrauToRadiano(double angulo)
        {
            return Math.PI * angulo / 180;
        }

        private void mnArquivoMostrador_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            try
            {
                pulso.Enabled = false;
                pbImagem.Image = null;
                GC.Collect(); // lixeiro do C# - coleta o lixo na RAM
                GC.WaitForPendingFinalizers(); // espera a coleta finalizar
                File.Delete(arquivo);
                File.Copy(openFileDialog1.FileName, arquivo);
                pulso.Enabled = true;
            }
            catch (Exception erro)
            {
                MessageBox.Show("Falha de arquivo:" + erro.Message);
                pulso.Enabled = true;
            }
        }
    }
}
