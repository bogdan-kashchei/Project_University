using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.OpenGl; // для работы с библиотекой OpenGL
using Tao.FreeGlut; // для работы с библиотекой FreeGLUT
using Tao.Platform.Windows; // для работы с элементом управления SimpleOpenGLControl


namespace Lab_5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();
        }

        // вспомогательные переменные - в них будут храниться обработанные значения, полученные при перетаскивании ползунков пользователем
        double a = 0, b = 0, c = -5, d = 0, zoom = 1;
        double p_1 = 1, p_2 = 1, p_3 = 0, p_4 = 1;       // переменные для работы с цветом     
        int os_x = 1, os_y = 0, os_z = 0;                // выбранные оси
        bool Wire = false;                               // режим сеточной визуализации



        private void Form1_Load(object sender, EventArgs e)
        {

            Glut.glutInit(); // инициализация библиотеки Glut
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE); // инициализация режима экрана
            Gl.glClearColor(1, 1, 1, 1); // установка цвета очистки экрана (RGBA)
            Gl.glViewport(0, 0, simpleOpenGlControl1.Width, simpleOpenGlControl1.Height);// установка порта вывода
            Gl.glMatrixMode(Gl.GL_PROJECTION);  // активация проекционной матрицы
            Gl.glLoadIdentity();// очистка матрицы
            // установка перспективы
            Glu.gluPerspective(45, (float)simpleOpenGlControl1.Width / (float)simpleOpenGlControl1.Height, 0.1, 200);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            // начальная настройка параметров openGL (тест глубины, первый источник света)
            if ((float)simpleOpenGlControl1.Width <= (float)simpleOpenGlControl1.Height)
            {
                //настройку 2D ортогональной проекции
                Glu.gluOrtho2D(0.0, 30.0f * (float)simpleOpenGlControl1.Height / (float)simpleOpenGlControl1.Width, 0.0, 30);
            }
            else
            {
                //настройку 2D ортогональной проекции
                Glu.gluOrtho2D(0.0, 30.0f * (float)simpleOpenGlControl1.Width / (float)simpleOpenGlControl1.Height, 0.0, 30);
                // Glu.gluOrtho2D помещает начало координат в левый нижний квадрат, а наблюдатель в этом случае находиться  на оси Z
            }
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            comboBox1.SelectedIndex = 0;// установка первых элементов в списках combobox
            //comboBox2.SelectedIndex = 0;
            timer1.Start();// активация таймера, вызывающего функцию для визуализации

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            Draw(); // вызываем функцию отрисовки сцены (код которой будет далее)

        }

        private void trackBar8_Scroll(object sender, EventArgs e) // X
        {
            a = (double)trackBar8.Value / 1000.0; // переводим значение, установившееся в элементе trackBar в необходимый нам формат
            label9.Text = a.ToString();   // записываем значение в поле перед бегунком

        }


        private void trackBar7_Scroll(object sender, EventArgs e)  // Y
        {
            b = (double)trackBar7.Value / 1000.0; // переводим значение, установившееся в элементе trackBar в необходимый нам формат
            label10.Text = b.ToString();   // записываем значение в поле перед бегунком

        }
        
        private void trackBar6_Scroll(object sender, EventArgs e)  // Z
        {
            c = (double)trackBar6.Value / 1000.0; // переводим значение, установившееся в элементе trackBar в необходимый нам формат
            label11.Text = c.ToString();   // записываем значение в поле перед бегунком

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void trackBar5_Scroll(object sender, EventArgs e)  // Вращение
        {
            d = (double)trackBar5.Value / 10.0; // переводим значение, установившееся в элементе trackBar в необходимый нам формат
            label12.Text = d.ToString();   // записываем значение в поле перед бегунком

        }



        private void trackBar9_Scroll(object sender, EventArgs e)  // Масштаб
        {
            zoom = (double)trackBar9.Value / 1000.0; // переводим значение, установившееся в элементе trackBar в необходимый нам формат
            label13.Text = zoom.ToString();   // записываем значение в поле перед бегунком

        }

                                 // Вариация цвета //

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            p_1 = (double)trackBar1.Value / 1000.0; // переводим значение, установившееся в элементе trackBar в необходимый нам формат
            label14.Text = p_1.ToString();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            p_2 = (double)trackBar2.Value / 1000.0; // переводим значение, установившееся в элементе trackBar в необходимый нам формат
            label15.Text = p_2.ToString();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            p_3 = (double)trackBar3.Value / 1000.0; // переводим значение, установившееся в элементе trackBar в необходимый нам формат
            label16.Text = p_3.ToString();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            p_4 = (double)trackBar4.Value / 1000.0; // переводим значение, установившееся в элементе trackBar в необходимый нам формат
            label17.Text = p_4.ToString();
        }

        // Вариация цвета //    

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (comboBox1.SelectedIndex) // устанавливаем необходимую ось 
            {
                case 0:
                    {
                        os_x = 1;
                        os_y = 0;
                        os_z = 0;
                        break;
                    }
                case 1:
                    {
                        os_x = 0;
                        os_y = 1;
                        os_z = 0;
                        break;
                    }
                case 2:
                    {
                        os_x = 0;
                        os_y = 0;
                        os_z = 1;
                        break;
                    }
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)// устанавливаем сеточный режим визуализации
            {
                Wire = true;
            }
            else// иначе - полигональная визуализация
            {
                Wire = false;
            }

        }

        private void Draw()// 
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT); // очистка буфера цвета и буфера глубины
            Gl.glClearColor(1, 1, 1, 1); // очищение текущей матрицы
            Gl.glLoadIdentity(); // помещаем состояние матрицы в стек матриц, дальнейшие трансформации затронут только визуализацию объекта
            Gl.glPushMatrix();
            Gl.glTranslated(a, b, c); //перемещение
            Gl.glRotated(d, os_x, os_y, os_z); // поворот по установленной оси
            Gl.glScaled(zoom, zoom, zoom); // масштабирование

            switch (comboBox2.SelectedIndex) // рисуем нужный объект, используя функции библиотеки GLUT
            {
                case 0:
                    {
                        if (Wire)// если установлен сеточный режим визуализации
                        {
                            Gl.glBegin(Gl.GL_LINE_LOOP);//face 1
                            Gl.glColor3ub(0, 0, 0);
                            Gl.glVertex3d(0.8, 0, 1);
                            Gl.glVertex3d(1, 0, 1);
                            Gl.glVertex3d(1, 1, 1);
                            Gl.glVertex3d(0, 1, 1);
                            Gl.glVertex3d(0, 0.8, 1);
                            Gl.glVertex3d(0.8, 0.8, 1);
                            Gl.glVertex3d(0.8, 0, 1);
                            Gl.glEnd();
                        }
                        else
                        {
                            Gl.glBegin(Gl.GL_LINE_LOOP);//face 1
                            Gl.glColor3d(p_3, p_3, p_3);
                            Gl.glColor3ub(0, 0, 0);
                            Gl.glColor3d(p_3, p_3, p_3);
                            Gl.glVertex3d(0.8, 0, 1);
                            Gl.glColor3d(p_3, p_2, p_3);
                            Gl.glVertex3d(1, 0, 1);
                            Gl.glVertex3d(1, 1, 1);
                            Gl.glVertex3d(0, 1, 1);
                            Gl.glColor3d(p_2, p_3, p_3);
                            Gl.glVertex3d(0, 0.8, 1);
                            Gl.glVertex3d(0.8, 0.8, 1);
                            Gl.glColor3d(p_3, p_2, p_3);
                            Gl.glVertex3d(0.8, 0, 1);
                            Gl.glEnd();



                        }
                        break;
                    }
                case 1:
                    {
                        if (Wire)// если установлен сеточный режим визуализации
                        {
                            Gl.glBegin(Gl.GL_LINE_LOOP);//face 2
                            Gl.glColor3ub(0, 0, 0);
                            Gl.glVertex3d(0.8, 0, 1);
                            Gl.glVertex3d(1, 0, 1);
                            Gl.glVertex3d(1, 0.6, 1);
                            Gl.glVertex3d(0.6, 1, 1);
                            Gl.glVertex3d(0, 1, 1);
                            Gl.glVertex3d(0, 0.8, 1);
                            Gl.glVertex3d(0.6, 0.8, 1);
                            Gl.glVertex3d(0.8, 0.6, 1);
                            Gl.glVertex3d(0.8, 0, 1);

                            Gl.glEnd();
                        }
                        else
                        {
                            Gl.glBegin(Gl.GL_LINE_LOOP);//face 2
                            Gl.glColor3ub(0, 0, 0);
                            Gl.glColor3d(p_2, p_3, p_3);
                            Gl.glVertex3d(0.8, 0, 1);
                            Gl.glVertex3d(1, 0, 1);
                            Gl.glColor3d(p_3, p_2, p_3);
                            Gl.glVertex3d(1, 0.6, 1);
                            Gl.glVertex3d(0.6, 1, 1);
                            Gl.glColor3d(p_3, p_3, p_2);
                            Gl.glVertex3d(0, 1, 1);
                            Gl.glVertex3d(0, 0.8, 1);
                            Gl.glColor3d(p_2, p_3, p_2);
                            Gl.glVertex3d(0.6, 0.8, 1);
                            Gl.glVertex3d(0.8, 0.6, 1);
                            Gl.glColor3d(p_1, p_3, p_2);
                            Gl.glVertex3d(0.8, 0, 1);

                            Gl.glEnd();
                        }
                        break;
                    }
                case 2:
                    {
                        if (Wire)// если установлен сеточный режим визуализации
                        {
                            Gl.glBegin(Gl.GL_LINE_LOOP);//face 3
                            Gl.glColor3ub(0, 0, 0);
                            Gl.glVertex3d(0.8, 0, 1);
                            Gl.glVertex3d(1, 0, 1);
                            Gl.glVertex3d(1, 0.6, 1);
                  
                            Gl.glVertex3d(0.95, 0.65, 1);
                            Gl.glVertex3d(0.9, 0.7, 1);
                            Gl.glVertex3d(0.85, 0.75, 1);
                            Gl.glVertex3d(0.8, 0.8, 1);
                            Gl.glVertex3d(0.75, 0.85, 1);
                            Gl.glVertex3d(0.7, 0.9, 1);
                            Gl.glVertex3d(0.65, 0.95, 1);
                            Gl.glVertex3d(0.6, 1, 1);
                
                            Gl.glVertex3d(0, 1, 1);
                            Gl.glVertex3d(0, 0.8, 1);
                            Gl.glVertex3d(0.8, 0.8, 1);
                            Gl.glVertex3d(0.8, 0, 1);
                            Gl.glEnd();
                        }

                        else
                        {
                            Gl.glBegin(Gl.GL_LINE_LOOP);//face 3
                            Gl.glColor3d(p_3, p_3, p_3);
                            Gl.glColor3ub(0, 0, 0);
                            Gl.glColor3d(p_3, p_3, p_3);
                            Gl.glVertex3d(0.8, 0, 1);
                            Gl.glColor3d(p_3, p_2, p_3);
                            Gl.glVertex3d(1, 0, 1);
                            Gl.glVertex3d(1, 1, 1);
                            Gl.glVertex3d(0, 1, 1);
                            Gl.glColor3d(p_2, p_3, p_3);
                            Gl.glVertex3d(0, 0.8, 1);
                            Gl.glVertex3d(0.8, 0.8, 1);
                            Gl.glColor3d(p_3, p_2, p_3);
                            Gl.glVertex3d(0.8, 0, 1);
                            Gl.glEnd();
                        }
                        break;
                    }
                case 3:
                    {
                        if (Wire)// если установлен сеточный режим визуализации
                        {
                            Gl.glColor3f((float)0.0, (float)0.0, (float)0.0);
                            Glut.glutWireCone(0.7, 1.2, 32, 32);// конус
                        }
                        else
                        {
                            Gl.glColor3f((float)p_1, (float)p_2, (float)p_3);
                            Glut.glutSolidCone(0.7, 1.2, 32, 32);
                        }
                        break;
                    }
                case 4:
                    {
                        if (Wire)// если установлен сеточный режим визуализации
                        {
                            Gl.glColor3f((float)0.0, (float)0.0, (float)0.0);
                            Glut.glutWireTorus(0.2, 0.8, 32, 32);// тор
                        }
                        else
                        {
                            Gl.glColor3f((float)0.7, (float)0.7, (float)0.7);
                            Glut.glutSolidTorus(0.2, 0.8, 32, 32);
                        }
                        break;
                    }
            }
             
            Gl.glPopMatrix();// возвращаем состояние матрицы
            Gl.glFlush();// завершаем рисование
            simpleOpenGlControl1.Invalidate();// обновляем элемент simpleOpenGlControl1
        }
    }
}
  
