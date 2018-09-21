using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
public class IniFileManager
{
    private static IniFileManager mInstance = null;

    private String m_strINIPath = Application.dataPath + "/../setting.ini";

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(String section, String key, String val, String filePath);
    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(String section, String key, String def, StringBuilder retVal, int size, String filePath);

    public static IniFileManager instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new IniFileManager();
            }
            return mInstance;
        }
    }

    private IniFileManager()
    {
        if (IsExistINI() == false)
        {
            WriteValue("COM", "PORT", "COM3" + Environment.NewLine);

            WriteValue("Speed & Time", "horizontalSpeed", "15");
            WriteValue("Speed & Time", "verticalSpeed", "10");
            WriteValue("Speed & Time", "alignSpeed", "5");
            WriteValue("Speed & Time", "vibrateSpeed", "25");
            WriteValue("Speed & Time", "vibrateDelayTime", "0.05" + Environment.NewLine);

            WriteValue("Angle", "xMin", "15");
            WriteValue("Angle", "xMax", "85");
            WriteValue("Angle", "yMin", "15");
            WriteValue("Angle", "yMax", "85");
            WriteValue("Angle", "vibrateMin", "-2");
            WriteValue("Angle", "vibrateMax", "2" + Environment.NewLine);
        }
    }

    public bool IsExistINI()
    {
        return File.Exists(m_strINIPath);
    }

    public void WriteValue(String strSection, String strKey, String strValue)
    {
        WritePrivateProfileString(strSection, strKey, strValue, m_strINIPath);
    }

    public void DeleteSection(String strSection)
    {
        WritePrivateProfileString(strSection, null, null, m_strINIPath);
    }

    public string ReadValue(String strSection, String Key)
    {
        StringBuilder strValue = new StringBuilder(255);
        int i = GetPrivateProfileString(strSection, Key, "", strValue, 255, m_strINIPath);
        return strValue.ToString();
    }
}

