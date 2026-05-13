namespace MarkingSystemV2.Services;

/// <summary>
/// 사용자 입력(바코드 등)에서 불가시 문자를 제거.
/// 파워포인트/엑셀/웹 페이지 복사 시 zero-width 문자가 끼어 들어와
/// 백엔드 매칭이 실패하는 사례 대응.
/// </summary>
internal static class InputSanitizer
{
    public static string Clean(string? input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        var sb = new System.Text.StringBuilder(input.Length);
        foreach (var c in input)
        {
            // U+200B ZWSP, U+200C ZWNJ, U+200D ZWJ, U+FEFF BOM
            if (c is '​' or '‌' or '‍' or '﻿') continue;
            sb.Append(c);
        }
        return sb.ToString().Trim();
    }
}
