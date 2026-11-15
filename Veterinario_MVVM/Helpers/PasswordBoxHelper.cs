// =========================================================================================
// Módulo Profesional: Desarrollo de Interfaces (2º DAM)
// Unidad Didáctica 04: Patrón MVVM en WPF
// Recurso de Apoyo: PasswordBoxHelper.cs
// =========================================================================================
// DESCRIPCIÓN:
//   En WPF, el control nativo PasswordBox no expone su propiedad "Password" como una
//   DependencyProperty por motivos de seguridad en memoria.
//
//   Esta clase estática implementa una "Attached Property" (Propiedad Adjunta) que permite
//   enlazar la contraseña de forma bidireccional con una propiedad del ViewModel en MVVM puro,
//   manteniendo el archivo code-behind (*.xaml.cs) completamente limpio (solo InitializeComponent).
//
// INSTRUCCIONES DE USO EN XAML:
//   1. En la etiqueta raíz de la ventana (ej. LoginView.xaml), declara el namespace:
//      xmlns:helpers="clr-namespace:Veterinario_MVVM.Helpers"  (o tu namespace de helpers)
//
//   2. En el control PasswordBox, añade los dos atributos adjuntos:
//      <PasswordBox x:Name="pwdPassword"
//                   helpers:PasswordBoxHelper.BindPassword="True"
//                   helpers:PasswordBoxHelper.BoundPassword="{Binding Password, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
//                   Height="36" TabIndex="2" />
// =========================================================================================

using System.Windows;
using System.Windows.Controls;

namespace Veterinario_MVVM.Helpers
{
    /// <summary>
    /// Propiedad adjunta (Attached Property) que permite enlazar de forma bidireccional
    /// la propiedad Password de un PasswordBox en MVVM respetando la seguridad y sin código en code-behind.
    /// </summary>
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BoundPassword",
                typeof(string),
                typeof(PasswordBoxHelper),
                new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static readonly DependencyProperty BindPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BindPassword",
                typeof(bool),
                typeof(PasswordBoxHelper),
                new PropertyMetadata(false, OnBindPasswordChanged));

        private static readonly DependencyProperty UpdatingPasswordProperty =
            DependencyProperty.RegisterAttached(
                "UpdatingPassword",
                typeof(bool),
                typeof(PasswordBoxHelper),
                new PropertyMetadata(false));

        public static void SetBindPassword(DependencyObject dp, bool value) => dp.SetValue(BindPasswordProperty, value);
        public static bool GetBindPassword(DependencyObject dp) => (bool)dp.GetValue(BindPasswordProperty);

        public static string GetBoundPassword(DependencyObject dp) => (string)dp.GetValue(BoundPasswordProperty);
        public static void SetBoundPassword(DependencyObject dp, string value) => dp.SetValue(BoundPasswordProperty, value);

        private static bool GetUpdatingPassword(DependencyObject dp) => (bool)dp.GetValue(UpdatingPasswordProperty);
        private static void SetUpdatingPassword(DependencyObject dp, bool value) => dp.SetValue(UpdatingPasswordProperty, value);

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox box)
            {
                var wasBound = (bool)e.OldValue;
                var needToBind = (bool)e.NewValue;

                if (wasBound)
                {
                    box.PasswordChanged -= HandlePasswordChanged;
                }

                if (needToBind)
                {
                    box.PasswordChanged += HandlePasswordChanged;
                }
            }
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox box)
            {
                SetUpdatingPassword(box, true);
                SetBoundPassword(box, box.Password);
                SetUpdatingPassword(box, false);
            }
        }

        private static void OnBoundPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox box)
            {
                if (!GetUpdatingPassword(box))
                {
                    box.Password = (string)e.NewValue;
                }
            }
        }
    }
}
