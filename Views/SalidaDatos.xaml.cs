namespace emedrandaS3Taller1.Views;

using System.Text;


[QueryProperty(nameof(Tipo), "Tipo")]
[QueryProperty(nameof(NumeroId), "NumeroId")]
[QueryProperty(nameof(Nombres), "Nombres")]
[QueryProperty(nameof(Apellidos), "Apellidos")]
[QueryProperty(nameof(Fecha), "Fecha")]
[QueryProperty(nameof(Correo), "Correo")]
[QueryProperty(nameof(Salario), "Salario")]

public partial class SalidaDatos : ContentPage
{
    public string? Tipo { get; set; }
    public string? NumeroId { get; set; }
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? Fecha { get; set; }
    public string? Correo { get; set; }
    public string? Salario { get; set; }

    decimal _salarioDecimal;
    decimal _aporteIess; // 9.45%


    public SalidaDatos()
	{
		InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        lblTipo.Text      = $"Tipo de Identificación: {Tipo}";
        lblNumero.Text    = $"Número: {NumeroId}";
        lblNombres.Text   = $"Nombres: {Nombres}";
        lblApellidos.Text = $"Apellidos: {Apellidos}";
        lblFecha.Text     = $"Fecha: {Fecha}";
        lblCorreo.Text    = $"Correo: {Correo}";

        decimal.TryParse((Salario ?? "0").Replace(',', '.'),
            System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture,
            out _salarioDecimal);

        _aporteIess = Math.Round(_salarioDecimal* 0.0945m, 2);

        lblSalario.Text = $"Salario: ${_salarioDecimal:N2}";
        lblIess.Text    = $"Aporte IESS (9,45%): ${_aporteIess:N2}";
    }

    async void BtnExportar_Clicked(object sender, EventArgs e)
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== Contacto ===");
        sb.AppendLine($"Tipo: {Tipo}");
        sb.AppendLine($"Número: {NumeroId}");
        sb.AppendLine($"Nombres: {Nombres}");
        sb.AppendLine($"Apellidos: {Apellidos}");
        sb.AppendLine($"Fecha: {Fecha}");
        sb.AppendLine($"Correo: {Correo}");
        sb.AppendLine($"Salario: {_salarioDecimal.ToString("N2")}");
        sb.AppendLine($"Aporte IESS (9,45%): {_aporteIess.ToString("N2")}");

        var fileName = $"Contacto_{Tipo}_{NumeroId}.txt".Replace(" ", "_");
        var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
        File.WriteAllText(filePath, sb.ToString());

        await DisplayAlert("Exportado", $"Archivo guardado:\n{filePath}", "OK");

        // Opción de compartir/guardar en otra ubicación
        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "Exportar contacto",
            File = new ShareFile(filePath)
        });
    }

}