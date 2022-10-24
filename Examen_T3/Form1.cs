using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Examen_T3
{
    public partial class Form1 : Form
    {
        // Campos
        private int borderRadius = 30;
        private int borderSize = 2;
        private Color borderColor = Color.SteelBlue; // cambiar de color_1.1


        // Constructor
        ArrayList lista;
        public Form1()
        {      
            InitializeComponent();
            lista = new ArrayList();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(borderSize);
            this.panelTitulo.BackColor = borderColor; // cambiando de color_1.1
            this.BackColor = borderColor; // Eliminar el parpadeo Blanco
        }


        // 02
        private void BTNingresar_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Ingrese un Valor");
                }
                else
                {
                    lista.Add(int.Parse(textBox1.Text));
                    textBox1.Text = String.Empty;
                    int i;
                    for (i = 0; 10 < lista.Count; i++)
                    {
                        listBox1.Items.Add(lista[i]);
                    }
                    lista.Add(int.Parse(listBox2.Text));
                    textBox1.Text = String.Empty;
                    int j;
                    for (j = 0; 10 < lista.Count; j++)
                    {
                        listBox1.Items.Add(lista[j]);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Digito Ingresado No es un Numero");
                textBox1.Focus();
            }        
        }

        // 02 btn descendente
        private void BTNordenar_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < lista.Count; i++)
            {
                lista.Sort();
                listBox1.Items.Add(lista[i]);
            }
        }

        // 02 btn descendente
        private void btn2_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < lista.Count; i++)
            {
                lista.Reverse();
                listBox2.Items.Add(lista[i]);
            }
        }

        private void BTNLimpiar_Click(object sender, EventArgs e)
        {
            listBox1.ClearSelected();
            listBox2.ClearSelected();
        }

        
        /// /////////////////////////////////////////////////////////////////////////////////////////
     
        // Método Mover / Arrastrar Formulario
        // Arrasta el -> Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x20000; // <--- Minimize borderless form from taskbar
                return cp;
            }
        }


        // Minimizar Formulario sin Bordes desde la Barra de tareas (Form1_MouseDown -> fue llamado desde propiedad)
        private void Form1_MouseDown(object sender, MouseEventArgs e)  // -> Para mover
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);    // -> mismo para Metros
        }

        private void panelTitulo_MouseDown(object sender, MouseEventArgs e) // -> Para mover
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);    // -> mismo para Metros
        }


        // Métodos Privados
        // Crear Ruta Redondeada
        private GraphicsPath GetRoundedPath(Rectangle rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }


        // Establecer Región y Borde Redondeado – Formulario
        private void FormRegionAndBorder(Form form, float radius, Graphics graph, Color borderColor, float borderSize)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                using (GraphicsPath roundPath = GetRoundedPath(form.ClientRectangle, radius))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                using (Matrix transform = new Matrix())
                {
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    form.Region = new Region(roundPath);
                    if (borderSize >= 1)
                    {
                        Rectangle rect = form.ClientRectangle;
                        float scaleX = 1.0F - ((borderSize + 1) / rect.Width);
                        float scaleY = 1.0F - ((borderSize + 1) / rect.Height);

                        transform.Scale(scaleX, scaleY);
                        transform.Translate(borderSize / 1.6F, borderSize / 1.6F);

                        graph.Transform = transform;
                        graph.DrawPath(penBorder, roundPath);  // esto da color al Marco (Color Azul) solo descomillar y se pone en Negro
                    }
                }
            }
        }


        // Establecer Región y Borde Redondeado – Panel Contenedor
        private void ControlRegionAndBorder(Control control, float radius, Graphics graph, Color borderColor)
        {
            using (GraphicsPath roundPath = GetRoundedPath(control.ClientRectangle, radius))
            using (Pen penBorder = new Pen(borderColor, 1))
            {
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                control.Region = new Region(roundPath);
                graph.DrawPath(penBorder, roundPath);
            }
        }


        // Dibujar Rutas – Esquinas del Formulario
        private void DrawPath(Rectangle rect, Graphics graph, Color color)
        {
            using (GraphicsPath roundPath = GetRoundedPath(rect,borderRadius))
            using (Pen penBorder = new Pen(color, 3))
            {
                graph.DrawPath(penBorder,roundPath);
            }
        }


        // Obtener Colores de los Limites Exteriores del Formulario
        private struct FormBoundsColors
        {
            public Color TopLeftColor;
            public Color TopRightColor;
            public Color BottomLeftColor;
            public Color BottomRightColor;
        }
        private FormBoundsColors GetFormBoundsColors()
        {
            var fbColor = new FormBoundsColors();
            using (var bmp = new Bitmap(1, 1))
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle rectBmp = new Rectangle(0, 0, 1, 1);

                //Top Left
                rectBmp.X = this.Bounds.X - 1;
                rectBmp.Y = this.Bounds.Y;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.TopLeftColor = bmp.GetPixel(0, 0);

                //Top Right
                rectBmp.X = this.Bounds.Right;
                rectBmp.Y = this.Bounds.Y;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.TopRightColor = bmp.GetPixel(0, 0);

                //Bottom Left
                rectBmp.X = this.Bounds.X;
                rectBmp.Y = this.Bounds.Bottom;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.BottomLeftColor = bmp.GetPixel(0, 0);

                //Bottom Right
                rectBmp.X = this.Bounds.Right;
                rectBmp.Y = this.Bounds.Bottom;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.BottomRightColor = bmp.GetPixel(0, 0);
            }
            return fbColor;
        }

        // Métodos de evento: Establecer Región Redondeado y Dibujar Borde + Suavizado (Formulario)
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            // borde exterior liso
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rectForm = this.ClientRectangle;
            int mWidht = rectForm.Width / 2;
            int mHeight = rectForm.Height / 2;
            var fbColors = GetFormBoundsColors();

            // arriba a la izquierda -> y el color
            DrawPath(rectForm,e.Graphics, fbColors.TopLeftColor);

            // parte superior derecha
            Rectangle rectopRight = new Rectangle(mWidht,rectForm.Y,mWidht,mHeight);
            DrawPath(rectopRight,e.Graphics, fbColors.TopRightColor);

            // Abajo a la izquierda
            Rectangle rectBottomLeft = new Rectangle(rectForm.X, rectForm.X + mHeight, mWidht, mHeight);
            DrawPath(rectBottomLeft, e.Graphics,fbColors.BottomLeftColor);

            // Bottom Right
            Rectangle rectBottomRight = new Rectangle(mWidht, rectForm.Y + mHeight, mWidht, mHeight);
            DrawPath(rectBottomRight, e.Graphics, fbColors.BottomRightColor);
        }

        // llamar desde Propiedad
        private void Form1_Paint(object sender, PaintEventArgs e)    //Métodos de evento
        {
            FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize); // -> el formulario
        }
        // Establecer Región Redondeado y Dibujar Borde Fino (Panel Contenedor)
        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {
            ControlRegionAndBorder(panelContainer,borderRadius-(borderSize/2), e.Graphics, borderColor);
        }


        // Actualizar Región y Borde
        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void Form1_Activated(object sender, EventArgs e)
        {
            this.Invalidate();
        }


        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BTNsalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
    }
}
