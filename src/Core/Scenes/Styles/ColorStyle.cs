namespace Xvg;

public enum ColorKind
{
  None,
  Zero, Black, White, Red, Green, Blue,
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

public static partial class ColorStyle
{
  public const ColorKind Default = ColorKind.Black;

  public const string None = "none";
  public const string Zero = "#00000000"; // black with no opacity (rrggbbaa)
  public const string Black = "#000";
  public const string White = "#fff";
  public const string Red = "#f00";
  public const string Green = "#0f0";
  public const string Blue = "#00f";

  #region [https://yeun.github.io/open-color/]

  public const string Gray0 = "#f8f9fa";
  public const string Gray1 = "#f1f3f5";
  public const string Gray2 = "#e9ecef";
  public const string Gray3 = "#dee2e6";
  public const string Gray4 = "#ced4da";
  public const string Gray5 = "#adb5bd";
  public const string Gray6 = "#868e96";
  public const string Gray7 = "#495057";
  public const string Gray8 = "#343a40";
  public const string Gray9 = "#212529";

  public const string Red0 = "#fff5f5";
  public const string Red1 = "#ffe3e3";
  public const string Red2 = "#ffc9c9";
  public const string Red3 = "#ffa8a8";
  public const string Red4 = "#ff8787";
  public const string Red5 = "#ff6b6b";
  public const string Red6 = "#fa5252";
  public const string Red7 = "#f03e3e";
  public const string Red8 = "#e03131";
  public const string Red9 = "#c92a2a";

  public const string Pink0 = "#fff0f6";
  public const string Pink1 = "#ffdeeb";
  public const string Pink2 = "#fcc2d7";
  public const string Pink3 = "#faa2c1";
  public const string Pink4 = "#f783ac";
  public const string Pink5 = "#f06595";
  public const string Pink6 = "#e64980";
  public const string Pink7 = "#d6336c";
  public const string Pink8 = "#c2255c";
  public const string Pink9 = "#a61e4d";

  public const string Grape0 = "#f8f0fc";
  public const string Grape1 = "#f3d9fa";
  public const string Grape2 = "#eebefa";
  public const string Grape3 = "#e599f7";
  public const string Grape4 = "#da77f2";
  public const string Grape5 = "#cc5de8";
  public const string Grape6 = "#be4bdb";
  public const string Grape7 = "#ae3ec9";
  public const string Grape8 = "#9c36b5";
  public const string Grape9 = "#862e9c";

  public const string Violet0 = "#f3f0ff";
  public const string Violet1 = "#e5dbff";
  public const string Violet2 = "#d0bfff";
  public const string Violet3 = "#b197fc";
  public const string Violet4 = "#9775fa";
  public const string Violet5 = "#845ef7";
  public const string Violet6 = "#7950f2";
  public const string Violet7 = "#7048e8";
  public const string Violet8 = "#6741d9";
  public const string Violet9 = "#5f3dc4";

  public const string Indigo0 = "#edf2ff";
  public const string Indigo1 = "#dbe4ff";
  public const string Indigo2 = "#bac8ff";
  public const string Indigo3 = "#91a7ff";
  public const string Indigo4 = "#748ffc";
  public const string Indigo5 = "#5c7cfa";
  public const string Indigo6 = "#4c6ef5";
  public const string Indigo7 = "#4263eb";
  public const string Indigo8 = "#3b5bdb";
  public const string Indigo9 = "#364fc7";

  public const string Blue0 = "#e7f5ff";
  public const string Blue1 = "#d0ebff";
  public const string Blue2 = "#a5d8ff";
  public const string Blue3 = "#74c0fc";
  public const string Blue4 = "#4dabf7";
  public const string Blue5 = "#339af0";
  public const string Blue6 = "#228be6";
  public const string Blue7 = "#1c7ed6";
  public const string Blue8 = "#1971c2";
  public const string Blue9 = "#1864ab";

  public const string Cyan0 = "#e3fafc";
  public const string Cyan1 = "#c5f6fa";
  public const string Cyan2 = "#99e9f2";
  public const string Cyan3 = "#66d9e8";
  public const string Cyan4 = "#3bc9db";
  public const string Cyan5 = "#22b8cf";
  public const string Cyan6 = "#15aabf";
  public const string Cyan7 = "#1098ad";
  public const string Cyan8 = "#0c8599";
  public const string Cyan9 = "#0b7285";

  public const string Teal0 = "#e6fcf5";
  public const string Teal1 = "#c3fae8";
  public const string Teal2 = "#96f2d7";
  public const string Teal3 = "#63e6be";
  public const string Teal4 = "#38d9a9";
  public const string Teal5 = "#20c997";
  public const string Teal6 = "#12b886";
  public const string Teal7 = "#0ca678";
  public const string Teal8 = "#099268";
  public const string Teal9 = "#087f5b";

  public const string Green0 = "#ebfbee";
  public const string Green1 = "#d3f9d8";
  public const string Green2 = "#b2f2bb";
  public const string Green3 = "#8ce99a";
  public const string Green4 = "#69db7c";
  public const string Green5 = "#51cf66";
  public const string Green6 = "#40c057";
  public const string Green7 = "#37b24d";
  public const string Green8 = "#2f9e44";
  public const string Green9 = "#2b8a3e";

  public const string Lime0 = "#f4fce3";
  public const string Lime1 = "#e9fac8";
  public const string Lime2 = "#d8f5a2";
  public const string Lime3 = "#c0eb75";
  public const string Lime4 = "#a9e34b";
  public const string Lime5 = "#94d82d";
  public const string Lime6 = "#82c91e";
  public const string Lime7 = "#74b816";
  public const string Lime8 = "#66a80f";
  public const string Lime9 = "#5c940d";

  public const string Yellow0 = "#fff9db";
  public const string Yellow1 = "#fff3bf";
  public const string Yellow2 = "#ffec99";
  public const string Yellow3 = "#ffe066";
  public const string Yellow4 = "#ffd43b";
  public const string Yellow5 = "#fcc419";
  public const string Yellow6 = "#fab005";
  public const string Yellow7 = "#f59f00";
  public const string Yellow8 = "#f08c00";
  public const string Yellow9 = "#e67700";

  public const string Orange0 = "#fff4e6";
  public const string Orange1 = "#ffe8cc";
  public const string Orange2 = "#ffd8a8";
  public const string Orange3 = "#ffc078";
  public const string Orange4 = "#ffa94d";
  public const string Orange5 = "#ff922b";
  public const string Orange6 = "#fd7e14";
  public const string Orange7 = "#f76707";
  public const string Orange8 = "#e8590c";
  public const string Orange9 = "#d9480f";

  #endregion

  public static string ToSvgStyle(this ColorKind self)
  {
    switch (self)
    {
      case ColorKind.None: return None;
      case ColorKind.Zero: return Zero;
      case ColorKind.Black: return Black;
      case ColorKind.White: return White;
      case ColorKind.Red: return Red;
      case ColorKind.Green: return Green;
      case ColorKind.Blue: return Blue;

      case ColorKind.Gray0: return Gray0;
      case ColorKind.Gray1: return Gray1;
      case ColorKind.Gray2: return Gray2;
      case ColorKind.Gray3: return Gray3;
      case ColorKind.Gray4: return Gray4;
      case ColorKind.Gray5: return Gray5;
      case ColorKind.Gray6: return Gray6;
      case ColorKind.Gray7: return Gray7;
      case ColorKind.Gray8: return Gray8;
      case ColorKind.Gray9: return Gray9;

      case ColorKind.Red0: return Red0;
      case ColorKind.Red1: return Red1;
      case ColorKind.Red2: return Red2;
      case ColorKind.Red3: return Red3;
      case ColorKind.Red4: return Red4;
      case ColorKind.Red5: return Red5;
      case ColorKind.Red6: return Red6;
      case ColorKind.Red7: return Red7;
      case ColorKind.Red8: return Red8;
      case ColorKind.Red9: return Red9;

      case ColorKind.Pink0: return Pink0;
      case ColorKind.Pink1: return Pink1;
      case ColorKind.Pink2: return Pink2;
      case ColorKind.Pink3: return Pink3;
      case ColorKind.Pink4: return Pink4;
      case ColorKind.Pink5: return Pink5;
      case ColorKind.Pink6: return Pink6;
      case ColorKind.Pink7: return Pink7;
      case ColorKind.Pink8: return Pink8;
      case ColorKind.Pink9: return Pink9;

      case ColorKind.Grape0: return Grape0;
      case ColorKind.Grape1: return Grape1;
      case ColorKind.Grape2: return Grape2;
      case ColorKind.Grape3: return Grape3;
      case ColorKind.Grape4: return Grape4;
      case ColorKind.Grape5: return Grape5;
      case ColorKind.Grape6: return Grape6;
      case ColorKind.Grape7: return Grape7;
      case ColorKind.Grape8: return Grape8;
      case ColorKind.Grape9: return Grape9;

      case ColorKind.Violet0: return Violet0;
      case ColorKind.Violet1: return Violet1;
      case ColorKind.Violet2: return Violet2;
      case ColorKind.Violet3: return Violet3;
      case ColorKind.Violet4: return Violet4;
      case ColorKind.Violet5: return Violet5;
      case ColorKind.Violet6: return Violet6;
      case ColorKind.Violet7: return Violet7;
      case ColorKind.Violet8: return Violet8;
      case ColorKind.Violet9: return Violet9;

      case ColorKind.Indigo0: return Indigo0;
      case ColorKind.Indigo1: return Indigo1;
      case ColorKind.Indigo2: return Indigo2;
      case ColorKind.Indigo3: return Indigo3;
      case ColorKind.Indigo4: return Indigo4;
      case ColorKind.Indigo5: return Indigo5;
      case ColorKind.Indigo6: return Indigo6;
      case ColorKind.Indigo7: return Indigo7;
      case ColorKind.Indigo8: return Indigo8;
      case ColorKind.Indigo9: return Indigo9;

      case ColorKind.Blue0: return Blue0;
      case ColorKind.Blue1: return Blue1;
      case ColorKind.Blue2: return Blue2;
      case ColorKind.Blue3: return Blue3;
      case ColorKind.Blue4: return Blue4;
      case ColorKind.Blue5: return Blue5;
      case ColorKind.Blue6: return Blue6;
      case ColorKind.Blue7: return Blue7;
      case ColorKind.Blue8: return Blue8;
      case ColorKind.Blue9: return Blue9;

      case ColorKind.Cyan0: return Cyan0;
      case ColorKind.Cyan1: return Cyan1;
      case ColorKind.Cyan2: return Cyan2;
      case ColorKind.Cyan3: return Cyan3;
      case ColorKind.Cyan4: return Cyan4;
      case ColorKind.Cyan5: return Cyan5;
      case ColorKind.Cyan6: return Cyan6;
      case ColorKind.Cyan7: return Cyan7;
      case ColorKind.Cyan8: return Cyan8;
      case ColorKind.Cyan9: return Cyan9;

      case ColorKind.Teal0: return Teal0;
      case ColorKind.Teal1: return Teal1;
      case ColorKind.Teal2: return Teal2;
      case ColorKind.Teal3: return Teal3;
      case ColorKind.Teal4: return Teal4;
      case ColorKind.Teal5: return Teal5;
      case ColorKind.Teal6: return Teal6;
      case ColorKind.Teal7: return Teal7;
      case ColorKind.Teal8: return Teal8;
      case ColorKind.Teal9: return Teal9;

      case ColorKind.Green0: return Green0;
      case ColorKind.Green1: return Green1;
      case ColorKind.Green2: return Green2;
      case ColorKind.Green3: return Green3;
      case ColorKind.Green4: return Green4;
      case ColorKind.Green5: return Green5;
      case ColorKind.Green6: return Green6;
      case ColorKind.Green7: return Green7;
      case ColorKind.Green8: return Green8;
      case ColorKind.Green9: return Green9;

      case ColorKind.Lime0: return Lime0;
      case ColorKind.Lime1: return Lime1;
      case ColorKind.Lime2: return Lime2;
      case ColorKind.Lime3: return Lime3;
      case ColorKind.Lime4: return Lime4;
      case ColorKind.Lime5: return Lime5;
      case ColorKind.Lime6: return Lime6;
      case ColorKind.Lime7: return Lime7;
      case ColorKind.Lime8: return Lime8;
      case ColorKind.Lime9: return Lime9;

      case ColorKind.Yellow0: return Yellow0;
      case ColorKind.Yellow1: return Yellow1;
      case ColorKind.Yellow2: return Yellow2;
      case ColorKind.Yellow3: return Yellow3;
      case ColorKind.Yellow4: return Yellow4;
      case ColorKind.Yellow5: return Yellow5;
      case ColorKind.Yellow6: return Yellow6;
      case ColorKind.Yellow7: return Yellow7;
      case ColorKind.Yellow8: return Yellow8;
      case ColorKind.Yellow9: return Yellow9;

      case ColorKind.Orange0: return Orange0;
      case ColorKind.Orange1: return Orange1;
      case ColorKind.Orange2: return Orange2;
      case ColorKind.Orange3: return Orange3;
      case ColorKind.Orange4: return Orange4;
      case ColorKind.Orange5: return Orange5;
      case ColorKind.Orange6: return Orange6;
      case ColorKind.Orange7: return Orange7;
      case ColorKind.Orange8: return Orange8;
      case ColorKind.Orange9: return Orange9;

      default: throw new NotSupportedException();
    }
  }
}
