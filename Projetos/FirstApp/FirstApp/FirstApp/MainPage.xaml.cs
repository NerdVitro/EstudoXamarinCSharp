using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FirstApp
{
    public partial class MainPage : ContentPage
    {
        private TimeSpan TmsTempoTimer; 
        private TimeSpan TmsTempoPausa;
        private TimeSpan TmsTempoAtual;
        private bool BlxRodandoTimer;
        private bool BlxParado;

        private Color ColorRolando = Color.FromRgb(2, 193, 199);
        private Color ColorPausado = Color.FromRgb(204, 4, 199);
        private Color ColorInicio = Color.FromRgb(2, 193, 199);
        private Color ColorTextos = Color.FromRgb(255, 255, 255);

        public MainPage()
        {
            InitializeComponent();
            TmsTempoTimer = TimeSpan.FromMinutes(25);
            TmsTempoPausa = TimeSpan.FromMinutes(5);
            TmsTempoAtual = TmsTempoTimer;
            AtualizarLabelTimer();
            AlterarCorTela(ColorInicio);
            AlterarCorTextos(ColorTextos);
        }

        private void AtualizarLabelTimer()
        {
            try
            {
                LblTimer.Text = TmsTempoAtual.ToString(@"mm\:ss");
            }
            catch (Exception ex)
            {
                MostrarErro(ex);
            }
        }
        private bool TimerTick()
        {
            try
            {
                if (!BlxRodandoTimer)
                {
                    return false;
                }

                TmsTempoAtual = TmsTempoAtual.Subtract(TimeSpan.FromSeconds(1));

                AtualizarLabelTimer();

                if (TmsTempoAtual.TotalSeconds <= 0)
                {
                    BlxRodandoTimer = false;
                    BtnIniciar.Text = "Iniciar";

                    if (!BlxParado)
                    {
                        BlxParado = true;
                        TmsTempoAtual = TmsTempoPausa;
                        DisplayAlert("Pomodoro", "Hora da pausa!", "OK");
                        AlterarCorTela(ColorPausado);
                    }
                    else
                    {
                        BlxParado = false;
                        TmsTempoAtual = TmsTempoTimer;
                        DisplayAlert("Pomodoro", "Fim da pausa. Volte ao trabalho!", "OK");
                        AlterarCorTela(ColorRolando);
                    }

                    AtualizarLabelTimer();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MostrarErro(ex);
                return true;
            }
        }
        private bool SetCustomTimes()
        {
            try
            {
                bool isWorkTimeValid = int.TryParse(EtyTempoTrabalho.Text, out int workMinutes);
                bool isBreakTimeValid = int.TryParse(EtyTempoPausa.Text, out int breakMinutes);

                if ((isWorkTimeValid && isBreakTimeValid) && (workMinutes > 0 && breakMinutes > 0))
                {
                    TmsTempoTimer = TimeSpan.FromMinutes(workMinutes);
                    TmsTempoPausa = TimeSpan.FromMinutes(breakMinutes);
                    TmsTempoAtual = TmsTempoTimer;
                    AtualizarLabelTimer();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MostrarErro(ex);
                return false;
            }
        }
        private void MostrarErro(Exception parException)
        {
            DisplayAlert("Erro", parException.Message, "Ok");
        }
        private void MostrarErro(string parException)
        {
            DisplayAlert("Erro", parException, "Ok");
        }
        private void AlterarCorTela(Color parColor)
        {
            try
            {
                this.BackgroundColor = parColor;
            }
            catch (Exception ex)
            {
                MostrarErro(ex);
            }
        }
        private void AlterarCorTextos(Color parColor)
        {
            try
            {
                LblTempoTrabalho.TextColor = parColor;
                EtyTempoTrabalho.TextColor = parColor;
                EtyTempoTrabalho.PlaceholderColor = parColor;

                LblTempoPausa.TextColor = parColor;
                EtyTempoPausa.TextColor = parColor;
                EtyTempoPausa.PlaceholderColor = parColor;

                LblTimer.TextColor = parColor;

                BtnIniciar.TextColor = parColor;
                BtnIniciar.BorderColor = parColor;
                BtnIniciar.BackgroundColor = Color.Transparent;

                BtnResetar.TextColor = parColor;
                BtnResetar.BorderColor = parColor;
                BtnResetar.BackgroundColor = Color.Transparent;
            }
            catch (Exception ex)
            {
                MostrarErro(ex);
            }
        }

        private void BtnIniciar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!SetCustomTimes())
                {
                    MostrarErro("Insira valores válidos para o tempo!");
                    return;
                }

                if (BlxRodandoTimer)
                {
                    BlxRodandoTimer = false;
                    BtnIniciar.Text = "Iniciar";
                    AlterarCorTela(ColorPausado);
                }
                else
                {
                    BlxRodandoTimer = true;
                    BtnIniciar.Text = "Pause";
                    AlterarCorTela(ColorRolando);
                    Device.StartTimer(TimeSpan.FromSeconds(1), TimerTick);
                }
            }
            catch (Exception ex)
            {
                MostrarErro(ex);
            }
        }
        private void BtnResetarar_Clicked(object sender, EventArgs e)
        {
            try
            {
                BlxRodandoTimer = false;
                BtnIniciar.Text = "Iniciar";
                AlterarCorTela(ColorPausado);
                TmsTempoAtual = TmsTempoTimer;
                BlxParado = false;
                AtualizarLabelTimer();
            }
            catch (Exception ex)
            {
                MostrarErro(ex);
            }
        }

        private void EtyTempoPausa_Focused(object sender, FocusEventArgs e)
        {
            EtyTempoPausa.CursorPosition = EtyTempoPausa.Text.Length;
            EtyTempoPausa.SelectionLength = EtyTempoPausa.Text.Length;
        }
        private void EtyTempoTrabalho_Focused(object sender, FocusEventArgs e)
        {
            EtyTempoTrabalho.CursorPosition = EtyTempoTrabalho.Text.Length;
            EtyTempoTrabalho.SelectionLength = EtyTempoTrabalho.Text.Length;
        }
    }
}
