namespace emedrandaS3Taller1.Views;

using System.Text.RegularExpressions;
public partial class Datos : ContentPage
{
	public Datos()
	{
		InitializeComponent();
	}

    void pkTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        var sel = pkTipo.SelectedItem?.ToString();
        txtCI.IsVisible = sel == "CI";
        txtRUC.IsVisible = sel == "RUC";
        txtPasaporte.IsVisible = sel == "Pasaporte";
    }

    // Solo d�gitos y m�ximo 10
    void DigitsOnly10_TextChanged(object sender, TextChangedEventArgs e)
    {
        var digits = Regex.Replace(e.NewTextValue ?? "", "[^0-9]", "");
        if (digits.Length > 10) digits = digits.Substring(0, 10);
        if (txtCI.Text != digits) txtCI.Text = digits;
    }

    // Solo d�gitos y m�ximo 13
    void DigitsOnly13_TextChanged(object sender, TextChangedEventArgs e)
    {
        var digits = Regex.Replace(e.NewTextValue ?? "", "[^0-9]", "");
        if (digits.Length > 13) digits = digits.Substring(0, 13);
        if (txtRUC.Text != digits) txtRUC.Text = digits;
    }

    async void BtnResumen_Clicked(object sender, EventArgs e)
    {
        // Validaciones m�nimas
        if (pkTipo.SelectedIndex < 0)
        {
            await DisplayAlert("Falta", "Selecciona el tipo de identificaci�n.", "OK");
            return;
        }

        string numeroId = "";
        var tipo = pkTipo.SelectedItem.ToString();

        if (tipo == "CI")
        {
            if (string.IsNullOrWhiteSpace(txtCI.Text) || txtCI.Text.Length != 10)
            {
                await DisplayAlert("CI inv�lida", "La CI debe tener 10 d�gitos.", "OK");
                return;
            }
            numeroId = txtCI.Text;
        }
        else if (tipo == "RUC")
        {
            if (string.IsNullOrWhiteSpace(txtRUC.Text) || txtRUC.Text.Length != 13)
            {
                await DisplayAlert("RUC inv�lido", "El RUC debe tener 13 d�gitos.", "OK");
                return;
            }
            numeroId = txtRUC.Text;
        }
        else // Pasaporte
        {
            if (string.IsNullOrWhiteSpace(txtPasaporte.Text))
            {
                await DisplayAlert("Dato faltante", "Ingresa el pasaporte.", "OK");
                return;
            }
            numeroId = txtPasaporte.Text.Trim();
        }

        if (string.IsNullOrWhiteSpace(txtNombres.Text) || string.IsNullOrWhiteSpace(txtApellidos.Text))
        {
            await DisplayAlert("Faltan nombres/apellidos", "Completa nombres y apellidos.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(txtCorreo.Text) || !Regex.IsMatch(txtCorreo.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            await DisplayAlert("Correo inv�lido", "Ingresa un correo v�lido.", "OK");
            return;
        }

        if (!decimal.TryParse((txtSalario.Text ?? "0").Replace(',', '.'), System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var salario) || salario < 0)
        {
            await DisplayAlert("Salario inv�lido", "Ingresa un salario num�rico v�lido.", "OK");
            return;
        }

        // Pasamos los datos por QueryString (sin modelos)
        var navDict = new Dictionary<string, object>
        {
            { "Tipo", tipo },
            { "NumeroId", numeroId },
            { "Nombres", txtNombres.Text!.Trim() },
            { "Apellidos", txtApellidos.Text!.Trim() },
            { "Fecha", dpFecha.Date.ToString("yyyy-MM-dd") },
            { "Correo", txtCorreo.Text!.Trim() },
            { "Salario", salario.ToString(System.Globalization.CultureInfo.InvariantCulture) }
        };

        await Shell.Current.GoToAsync(nameof(SalidaDatos), navDict);
    }

}