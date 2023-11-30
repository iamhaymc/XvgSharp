namespace Xvg;

/// <summary>
/// https://yeun.github.io/open-color/
/// </summary>
public struct OpenColor : IColor
{
  public string Hex { get; set; }
  internal OpenColor(string hex) => Hex = hex;
  public string ToHex(bool alpha = true) => Hex;
}

public enum OpenColorType
{
  Black, White,
  Gray0, Gray1, Gray2, Gray3, Gray4, Gray5, Gray6, Gray7, Gray8, Gray9,
  Red0, Red1, Red2, Red3, Red4, Red5, Red6, Red7, Red8, Red9,
  Pink0, Pink1, Pink2, Pink3, Pink4, Pink5, Pink6, Pink7, Pink8, Pink9,
  Grape0, Grape1, Grape2, Grape3, Grape4, Grape5, Grape6, Grape7, Grape8, Grape9,
  Violet0, Violet1, Violet2, Violet3, Violet4, Violet5, Violet6, Violet7, Violet8, Violet9,
  Indigo0, Indigo1, Indigo2, Indigo3, Indigo4, Indigo5, Indigo6, Indigo7, Indigo8, Indigo9,
  Blue0, Blue1, Blue2, Blue3, Blue4, Blue5, Blue6, Blue7, Blue8, Blue9,
  Cyan0, Cyan1, Cyan2, Cyan3, Cyan4, Cyan5, Cyan6, Cyan7, Cyan8, Cyan9,
  Teal0, Teal1, Teal2, Teal3, Teal4, Teal5, Teal6, Teal7, Teal8, Teal9,
  Green0, Green1, Green2, Green3, Green4, Green5, Green6, Green7, Green8, Green9,
  Lime0, Lime1, Lime2, Lime3, Lime4, Lime5, Lime6, Lime7, Lime8, Lime9,
  Yellow0, Yellow1, Yellow2, Yellow3, Yellow4, Yellow5, Yellow6, Yellow7, Yellow8, Yellow9,
  Orange0, Orange1, Orange2, Orange3, Orange4, Orange5, Orange6, Orange7, Orange8, Orange9
}

public static partial class OpenColorTypeExtensions
{
  public static readonly OpenColor Black = new OpenColor("#000");
  public static readonly OpenColor White = new OpenColor("#fff");

  public static readonly OpenColor Gray0 = new OpenColor("#f8f9fa");
  public static readonly OpenColor Gray1 = new OpenColor("#f1f3f5");
  public static readonly OpenColor Gray2 = new OpenColor("#e9ecef");
  public static readonly OpenColor Gray3 = new OpenColor("#dee2e6");
  public static readonly OpenColor Gray4 = new OpenColor("#ced4da");
  public static readonly OpenColor Gray5 = new OpenColor("#adb5bd");
  public static readonly OpenColor Gray6 = new OpenColor("#868e96");
  public static readonly OpenColor Gray7 = new OpenColor("#495057");
  public static readonly OpenColor Gray8 = new OpenColor("#343a40");
  public static readonly OpenColor Gray9 = new OpenColor("#212529");

  public static readonly OpenColor Red0 = new OpenColor("#fff5f5");
  public static readonly OpenColor Red1 = new OpenColor("#ffe3e3");
  public static readonly OpenColor Red2 = new OpenColor("#ffc9c9");
  public static readonly OpenColor Red3 = new OpenColor("#ffa8a8");
  public static readonly OpenColor Red4 = new OpenColor("#ff8787");
  public static readonly OpenColor Red5 = new OpenColor("#ff6b6b");
  public static readonly OpenColor Red6 = new OpenColor("#fa5252");
  public static readonly OpenColor Red7 = new OpenColor("#f03e3e");
  public static readonly OpenColor Red8 = new OpenColor("#e03131");
  public static readonly OpenColor Red9 = new OpenColor("#c92a2a");

  public static readonly OpenColor Pink0 = new OpenColor("#fff0f6");
  public static readonly OpenColor Pink1 = new OpenColor("#ffdeeb");
  public static readonly OpenColor Pink2 = new OpenColor("#fcc2d7");
  public static readonly OpenColor Pink3 = new OpenColor("#faa2c1");
  public static readonly OpenColor Pink4 = new OpenColor("#f783ac");
  public static readonly OpenColor Pink5 = new OpenColor("#f06595");
  public static readonly OpenColor Pink6 = new OpenColor("#e64980");
  public static readonly OpenColor Pink7 = new OpenColor("#d6336c");
  public static readonly OpenColor Pink8 = new OpenColor("#c2255c");
  public static readonly OpenColor Pink9 = new OpenColor("#a61e4d");

  public static readonly OpenColor Grape0 = new OpenColor("#f8f0fc");
  public static readonly OpenColor Grape1 = new OpenColor("#f3d9fa");
  public static readonly OpenColor Grape2 = new OpenColor("#eebefa");
  public static readonly OpenColor Grape3 = new OpenColor("#e599f7");
  public static readonly OpenColor Grape4 = new OpenColor("#da77f2");
  public static readonly OpenColor Grape5 = new OpenColor("#cc5de8");
  public static readonly OpenColor Grape6 = new OpenColor("#be4bdb");
  public static readonly OpenColor Grape7 = new OpenColor("#ae3ec9");
  public static readonly OpenColor Grape8 = new OpenColor("#9c36b5");
  public static readonly OpenColor Grape9 = new OpenColor("#862e9c");

  public static readonly OpenColor Violet0 = new OpenColor("#f3f0ff");
  public static readonly OpenColor Violet1 = new OpenColor("#e5dbff");
  public static readonly OpenColor Violet2 = new OpenColor("#d0bfff");
  public static readonly OpenColor Violet3 = new OpenColor("#b197fc");
  public static readonly OpenColor Violet4 = new OpenColor("#9775fa");
  public static readonly OpenColor Violet5 = new OpenColor("#845ef7");
  public static readonly OpenColor Violet6 = new OpenColor("#7950f2");
  public static readonly OpenColor Violet7 = new OpenColor("#7048e8");
  public static readonly OpenColor Violet8 = new OpenColor("#6741d9");
  public static readonly OpenColor Violet9 = new OpenColor("#5f3dc4");

  public static readonly OpenColor Indigo0 = new OpenColor("#edf2ff");
  public static readonly OpenColor Indigo1 = new OpenColor("#dbe4ff");
  public static readonly OpenColor Indigo2 = new OpenColor("#bac8ff");
  public static readonly OpenColor Indigo3 = new OpenColor("#91a7ff");
  public static readonly OpenColor Indigo4 = new OpenColor("#748ffc");
  public static readonly OpenColor Indigo5 = new OpenColor("#5c7cfa");
  public static readonly OpenColor Indigo6 = new OpenColor("#4c6ef5");
  public static readonly OpenColor Indigo7 = new OpenColor("#4263eb");
  public static readonly OpenColor Indigo8 = new OpenColor("#3b5bdb");
  public static readonly OpenColor Indigo9 = new OpenColor("#364fc7");

  public static readonly OpenColor Blue0 = new OpenColor("#e7f5ff");
  public static readonly OpenColor Blue1 = new OpenColor("#d0ebff");
  public static readonly OpenColor Blue2 = new OpenColor("#a5d8ff");
  public static readonly OpenColor Blue3 = new OpenColor("#74c0fc");
  public static readonly OpenColor Blue4 = new OpenColor("#4dabf7");
  public static readonly OpenColor Blue5 = new OpenColor("#339af0");
  public static readonly OpenColor Blue6 = new OpenColor("#228be6");
  public static readonly OpenColor Blue7 = new OpenColor("#1c7ed6");
  public static readonly OpenColor Blue8 = new OpenColor("#1971c2");
  public static readonly OpenColor Blue9 = new OpenColor("#1864ab");

  public static readonly OpenColor Cyan0 = new OpenColor("#e3fafc");
  public static readonly OpenColor Cyan1 = new OpenColor("#c5f6fa");
  public static readonly OpenColor Cyan2 = new OpenColor("#99e9f2");
  public static readonly OpenColor Cyan3 = new OpenColor("#66d9e8");
  public static readonly OpenColor Cyan4 = new OpenColor("#3bc9db");
  public static readonly OpenColor Cyan5 = new OpenColor("#22b8cf");
  public static readonly OpenColor Cyan6 = new OpenColor("#15aabf");
  public static readonly OpenColor Cyan7 = new OpenColor("#1098ad");
  public static readonly OpenColor Cyan8 = new OpenColor("#0c8599");
  public static readonly OpenColor Cyan9 = new OpenColor("#0b7285");

  public static readonly OpenColor Teal0 = new OpenColor("#e6fcf5");
  public static readonly OpenColor Teal1 = new OpenColor("#c3fae8");
  public static readonly OpenColor Teal2 = new OpenColor("#96f2d7");
  public static readonly OpenColor Teal3 = new OpenColor("#63e6be");
  public static readonly OpenColor Teal4 = new OpenColor("#38d9a9");
  public static readonly OpenColor Teal5 = new OpenColor("#20c997");
  public static readonly OpenColor Teal6 = new OpenColor("#12b886");
  public static readonly OpenColor Teal7 = new OpenColor("#0ca678");
  public static readonly OpenColor Teal8 = new OpenColor("#099268");
  public static readonly OpenColor Teal9 = new OpenColor("#087f5b");

  public static readonly OpenColor Green0 = new OpenColor("#ebfbee");
  public static readonly OpenColor Green1 = new OpenColor("#d3f9d8");
  public static readonly OpenColor Green2 = new OpenColor("#b2f2bb");
  public static readonly OpenColor Green3 = new OpenColor("#8ce99a");
  public static readonly OpenColor Green4 = new OpenColor("#69db7c");
  public static readonly OpenColor Green5 = new OpenColor("#51cf66");
  public static readonly OpenColor Green6 = new OpenColor("#40c057");
  public static readonly OpenColor Green7 = new OpenColor("#37b24d");
  public static readonly OpenColor Green8 = new OpenColor("#2f9e44");
  public static readonly OpenColor Green9 = new OpenColor("#2b8a3e");

  public static readonly OpenColor Lime0 = new OpenColor("#f4fce3");
  public static readonly OpenColor Lime1 = new OpenColor("#e9fac8");
  public static readonly OpenColor Lime2 = new OpenColor("#d8f5a2");
  public static readonly OpenColor Lime3 = new OpenColor("#c0eb75");
  public static readonly OpenColor Lime4 = new OpenColor("#a9e34b");
  public static readonly OpenColor Lime5 = new OpenColor("#94d82d");
  public static readonly OpenColor Lime6 = new OpenColor("#82c91e");
  public static readonly OpenColor Lime7 = new OpenColor("#74b816");
  public static readonly OpenColor Lime8 = new OpenColor("#66a80f");
  public static readonly OpenColor Lime9 = new OpenColor("#5c940d");

  public static readonly OpenColor Yellow0 = new OpenColor("#fff9db");
  public static readonly OpenColor Yellow1 = new OpenColor("#fff3bf");
  public static readonly OpenColor Yellow2 = new OpenColor("#ffec99");
  public static readonly OpenColor Yellow3 = new OpenColor("#ffe066");
  public static readonly OpenColor Yellow4 = new OpenColor("#ffd43b");
  public static readonly OpenColor Yellow5 = new OpenColor("#fcc419");
  public static readonly OpenColor Yellow6 = new OpenColor("#fab005");
  public static readonly OpenColor Yellow7 = new OpenColor("#f59f00");
  public static readonly OpenColor Yellow8 = new OpenColor("#f08c00");
  public static readonly OpenColor Yellow9 = new OpenColor("#e67700");

  public static readonly OpenColor Orange0 = new OpenColor("#fff4e6");
  public static readonly OpenColor Orange1 = new OpenColor("#ffe8cc");
  public static readonly OpenColor Orange2 = new OpenColor("#ffd8a8");
  public static readonly OpenColor Orange3 = new OpenColor("#ffc078");
  public static readonly OpenColor Orange4 = new OpenColor("#ffa94d");
  public static readonly OpenColor Orange5 = new OpenColor("#ff922b");
  public static readonly OpenColor Orange6 = new OpenColor("#fd7e14");
  public static readonly OpenColor Orange7 = new OpenColor("#f76707");
  public static readonly OpenColor Orange8 = new OpenColor("#e8590c");
  public static readonly OpenColor Orange9 = new OpenColor("#d9480f");

  public static OpenColor ToColor(this OpenColorType self)
  {
    switch (self)
    {
      case OpenColorType.Black: return Black;
      case OpenColorType.White: return White;

      case OpenColorType.Gray0: return Gray0;
      case OpenColorType.Gray1: return Gray1;
      case OpenColorType.Gray2: return Gray2;
      case OpenColorType.Gray3: return Gray3;
      case OpenColorType.Gray4: return Gray4;
      case OpenColorType.Gray5: return Gray5;
      case OpenColorType.Gray6: return Gray6;
      case OpenColorType.Gray7: return Gray7;
      case OpenColorType.Gray8: return Gray8;
      case OpenColorType.Gray9: return Gray9;

      case OpenColorType.Red0: return Red0;
      case OpenColorType.Red1: return Red1;
      case OpenColorType.Red2: return Red2;
      case OpenColorType.Red3: return Red3;
      case OpenColorType.Red4: return Red4;
      case OpenColorType.Red5: return Red5;
      case OpenColorType.Red6: return Red6;
      case OpenColorType.Red7: return Red7;
      case OpenColorType.Red8: return Red8;
      case OpenColorType.Red9: return Red9;

      case OpenColorType.Pink0: return Pink0;
      case OpenColorType.Pink1: return Pink1;
      case OpenColorType.Pink2: return Pink2;
      case OpenColorType.Pink3: return Pink3;
      case OpenColorType.Pink4: return Pink4;
      case OpenColorType.Pink5: return Pink5;
      case OpenColorType.Pink6: return Pink6;
      case OpenColorType.Pink7: return Pink7;
      case OpenColorType.Pink8: return Pink8;
      case OpenColorType.Pink9: return Pink9;

      case OpenColorType.Grape0: return Grape0;
      case OpenColorType.Grape1: return Grape1;
      case OpenColorType.Grape2: return Grape2;
      case OpenColorType.Grape3: return Grape3;
      case OpenColorType.Grape4: return Grape4;
      case OpenColorType.Grape5: return Grape5;
      case OpenColorType.Grape6: return Grape6;
      case OpenColorType.Grape7: return Grape7;
      case OpenColorType.Grape8: return Grape8;
      case OpenColorType.Grape9: return Grape9;

      case OpenColorType.Violet0: return Violet0;
      case OpenColorType.Violet1: return Violet1;
      case OpenColorType.Violet2: return Violet2;
      case OpenColorType.Violet3: return Violet3;
      case OpenColorType.Violet4: return Violet4;
      case OpenColorType.Violet5: return Violet5;
      case OpenColorType.Violet6: return Violet6;
      case OpenColorType.Violet7: return Violet7;
      case OpenColorType.Violet8: return Violet8;
      case OpenColorType.Violet9: return Violet9;

      case OpenColorType.Indigo0: return Indigo0;
      case OpenColorType.Indigo1: return Indigo1;
      case OpenColorType.Indigo2: return Indigo2;
      case OpenColorType.Indigo3: return Indigo3;
      case OpenColorType.Indigo4: return Indigo4;
      case OpenColorType.Indigo5: return Indigo5;
      case OpenColorType.Indigo6: return Indigo6;
      case OpenColorType.Indigo7: return Indigo7;
      case OpenColorType.Indigo8: return Indigo8;
      case OpenColorType.Indigo9: return Indigo9;

      case OpenColorType.Blue0: return Blue0;
      case OpenColorType.Blue1: return Blue1;
      case OpenColorType.Blue2: return Blue2;
      case OpenColorType.Blue3: return Blue3;
      case OpenColorType.Blue4: return Blue4;
      case OpenColorType.Blue5: return Blue5;
      case OpenColorType.Blue6: return Blue6;
      case OpenColorType.Blue7: return Blue7;
      case OpenColorType.Blue8: return Blue8;
      case OpenColorType.Blue9: return Blue9;

      case OpenColorType.Cyan0: return Cyan0;
      case OpenColorType.Cyan1: return Cyan1;
      case OpenColorType.Cyan2: return Cyan2;
      case OpenColorType.Cyan3: return Cyan3;
      case OpenColorType.Cyan4: return Cyan4;
      case OpenColorType.Cyan5: return Cyan5;
      case OpenColorType.Cyan6: return Cyan6;
      case OpenColorType.Cyan7: return Cyan7;
      case OpenColorType.Cyan8: return Cyan8;
      case OpenColorType.Cyan9: return Cyan9;

      case OpenColorType.Teal0: return Teal0;
      case OpenColorType.Teal1: return Teal1;
      case OpenColorType.Teal2: return Teal2;
      case OpenColorType.Teal3: return Teal3;
      case OpenColorType.Teal4: return Teal4;
      case OpenColorType.Teal5: return Teal5;
      case OpenColorType.Teal6: return Teal6;
      case OpenColorType.Teal7: return Teal7;
      case OpenColorType.Teal8: return Teal8;
      case OpenColorType.Teal9: return Teal9;

      case OpenColorType.Green0: return Green0;
      case OpenColorType.Green1: return Green1;
      case OpenColorType.Green2: return Green2;
      case OpenColorType.Green3: return Green3;
      case OpenColorType.Green4: return Green4;
      case OpenColorType.Green5: return Green5;
      case OpenColorType.Green6: return Green6;
      case OpenColorType.Green7: return Green7;
      case OpenColorType.Green8: return Green8;
      case OpenColorType.Green9: return Green9;

      case OpenColorType.Lime0: return Lime0;
      case OpenColorType.Lime1: return Lime1;
      case OpenColorType.Lime2: return Lime2;
      case OpenColorType.Lime3: return Lime3;
      case OpenColorType.Lime4: return Lime4;
      case OpenColorType.Lime5: return Lime5;
      case OpenColorType.Lime6: return Lime6;
      case OpenColorType.Lime7: return Lime7;
      case OpenColorType.Lime8: return Lime8;
      case OpenColorType.Lime9: return Lime9;

      case OpenColorType.Yellow0: return Yellow0;
      case OpenColorType.Yellow1: return Yellow1;
      case OpenColorType.Yellow2: return Yellow2;
      case OpenColorType.Yellow3: return Yellow3;
      case OpenColorType.Yellow4: return Yellow4;
      case OpenColorType.Yellow5: return Yellow5;
      case OpenColorType.Yellow6: return Yellow6;
      case OpenColorType.Yellow7: return Yellow7;
      case OpenColorType.Yellow8: return Yellow8;
      case OpenColorType.Yellow9: return Yellow9;

      case OpenColorType.Orange0: return Orange0;
      case OpenColorType.Orange1: return Orange1;
      case OpenColorType.Orange2: return Orange2;
      case OpenColorType.Orange3: return Orange3;
      case OpenColorType.Orange4: return Orange4;
      case OpenColorType.Orange5: return Orange5;
      case OpenColorType.Orange6: return Orange6;
      case OpenColorType.Orange7: return Orange7;
      case OpenColorType.Orange8: return Orange8;
      case OpenColorType.Orange9: return Orange9;

      default: throw new NotSupportedException();
    }
  }
}
