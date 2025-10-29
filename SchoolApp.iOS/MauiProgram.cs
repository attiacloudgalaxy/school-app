using Microsoft.Extensions.Logging;
using SchoolApp.iOS.Services;
using SchoolApp.iOS.Pages;

namespace SchoolApp.iOS;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Register API Service
		builder.Services.AddSingleton<SchoolApiService>();

		// Register Pages
		builder.Services.AddTransient<HomePage>();
		builder.Services.AddTransient<ClassesPage>();
		builder.Services.AddTransient<StudentsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
