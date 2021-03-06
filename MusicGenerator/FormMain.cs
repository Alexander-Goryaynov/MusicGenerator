﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicGenerator
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonNote_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var octave = groupBoxOctave
                .Controls
                .OfType<RadioButton>()
                .FirstOrDefault(x => x.Checked)
                .Name
                .Replace("radioButton", "");
            var length = groupBoxLength
                .Controls
                .OfType<RadioButton>()
                .FirstOrDefault(x => x.Checked)
                .Name
                .Replace("radioButton", "");
            var noteName = button.Name.Replace("button", "");
            richTextBoxResult.Text += noteName;
            if (noteName == "Pause")
            {
                richTextBoxResult.Text += " ";
            }
            else
            {
                richTextBoxResult.Text += octave + " ";
            }            
            richTextBoxResult.Text += length + " ";
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            radioButtonTwo.Checked = true;
            radioButtonFirst.Checked = true;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            richTextBoxResult.Text = "";
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextBoxResult.Text))
            {
                return;
            }
            using (var dialog = new SaveFileDialog { Filter = "txt | *.txt" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllLines(dialog.FileName,
                            richTextBoxResult.Text.Split(new string[] { "\n" },
                            StringSplitOptions.RemoveEmptyEntries));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Ошибка записи");
                        return;
                    }
                    MessageBox.Show("Запись прошла успешно");
                }
            }
        }

        private void buttonDelLastNote_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(richTextBoxResult.Text))
                {
                    return;
                }
                var words = richTextBoxResult.Text.Split(new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int i = 0; i < 2; i++)
                {
                    words.RemoveAt(words.Count - 1);
                }
                richTextBoxResult.Text = string.Join(" ", words) + " ";
            } 
            catch 
            {
                return;
            }
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            richTextBoxResult.Text = Generator.GenerateMusic();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextBoxResult.Text)) 
            {
                MessageBox.Show("Не введены ноты", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int bpm = Convert.ToInt32(textBoxBpm.Text);
            if (bpm < 30 || bpm > 240)
            {
                MessageBox.Show("Допустима скорость от 30 до 240 уд/мин", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                Player.Play(Player.ParseInput(richTextBoxResult.Text), bpm);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка воспроизведения",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
