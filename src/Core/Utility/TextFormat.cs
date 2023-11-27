using Xvg.Humanize;

namespace Xvg;
public static class TextFormat
{
  public static string ToTruncated(string value, int length, string trail = "...")
    => value.Truncate(length, trail);

  /// <summary>
  /// Converts a string to "Titlecase"
  /// </summary>
  public static string ToTitleCase(string value)
    => value.Titleize();

  /// <summary>
  /// Converts a string to "PascalCase"
  /// </summary>
  public static string ToPascalCase(string value)
    => value.Replace("-", "_").Pascalize();

  /// <summary>
  /// Converts a string to "camelCase"
  /// </summary>
  public static string ToCamelCase(string value)
    => value.Replace("-", "_").Camelize();

  /// <summary>
  /// Converts a string to "kebab-case"
  /// </summary>
  public static string ToKebabCase(string value)
    => value.Kebaberize();

  /// <summary>
  /// Converts a string to "snake_case"
  /// </summary>
  public static string ToSnakeCase(string value)
    => value.ToLowerInvariant().Underscore();

  public static string ToMetric(double value)
    => value.ToMetric();

  public static double FromMetric(string value)
    => value.FromMetric();

  public static string ToPlural(string value)
  => value.Pluralize();

  public static string ToSingular(string value)
    => value.Singularize();

  public static string ToTime(TimeSpan time)
    => time.Humanize();

  public static string ToSeconds(TimeSpan time)
    => time.Humanize(maxUnit: Xvg.Humanize.Localisation.TimeUnit.Second, precision: 3);

  public static string ToMilliseconds(TimeSpan time)
    => time.Humanize(maxUnit: Xvg.Humanize.Localisation.TimeUnit.Millisecond, precision: 3);

  public static string ToUniveralDate(DateTime date)
    => date.ToUniversalTime().Humanize();
}
