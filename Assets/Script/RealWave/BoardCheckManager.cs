using System.Runtime.InteropServices;
using System;
using UnityEngine;
public class BoardCheckManager
{
    private static int IO_Index = 661;
    private static int IO_Data = 662;
    private static int hio;
    public byte vidH;
    public byte vidL;
    public byte cidH;
    public byte cidL;
    public byte reg24;
    public byte reg26;
    public uint nResult = 0;
    [DllImport("inpoutx64.dll", EntryPoint = "IsInpOutDriverOpen")]
    private static extern UInt32 IsInpOutDriverOpen_x64();

    [DllImport("inpoutx64.dll", EntryPoint = "DlPortWritePortUshort")]
    private static extern void DlPortWritePortUshort(int PortAddress, ushort Data);

    [DllImport("inpoutx64.dll", EntryPoint = "Inp32")]
    private static extern byte Inp32(int PortAddress);


    public BoardCheckManager()
    {
        try
        {
            nResult = IsInpOutDriverOpen_x64();
            if (nResult != 0)
            {
                Debug.Log("오픈");
            }
        }
        catch (Exception e)
        {

        }
    }
    public byte Read_Reg(int i)
    {
        byte d;
        d = Inp32(i);

        return d;
    }


    public void Write_Reg(int i, ushort d)
    {
        DlPortWritePortUshort(i, d);
    }

    public bool Check_System()
    {
        try
        {
            Write_Reg(0x2e, 0x87);
            Write_Reg(0x2e, 0x87);
            Write_Reg(0x2e, 0x20);
            cidH = Read_Reg(0x2f);
            Write_Reg(0x2e, 0x21);
            cidL = Read_Reg(0x2f);
            Write_Reg(0x2e, 0xaa);
            Write_Reg(0x0295, 0x4e);
            Write_Reg(0x0296, 0x80);
            Write_Reg(0x0295, 0x4f);
            vidH = Read_Reg(0x0296);
            Write_Reg(0x0295, 0x4e);
            Write_Reg(0x0296, 0x00);
            Write_Reg(0x0295, 0x4f);
            vidL = Read_Reg(0x0296);

            Write_Reg(0x0295, 0x24);
            reg24 = Read_Reg(0x0296);
            Write_Reg(0x0295, 0x26);
            reg26 = Read_Reg(0x0296);

            Debug.Log("읽은값 : " + cidH + " : " + cidL + " : " + vidH + " : " + vidL + " : " + reg24 + " : " + reg26);
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            return false;
        }
    }
    public bool Check_test()
    {
        try
        {
            reg24 = 150;
            vidH = 134;
            reg26 = 150;
            vidL = 18;
            cidH = 0x00;
            cidL = 3;
            Debug.Log("테스트코드 : " + cidH + " : " + cidL + " : " + vidH + " : " + vidL + " : " + reg24 + " : " + reg26);

            return true;
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            return false;
        }
    }

}
