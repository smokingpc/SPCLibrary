﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpcLibrary.DeviceIoControl
{
    public enum DEVICE_TYPE : UInt32
    {
        BEEP = 0x00000001,
        CD_ROM = 0x00000002,
        CD_ROM_FILE_SYSTEM = 0x00000003,
        CONTROLLER = 0x00000004,
        DATALINK = 0x00000005,
        DFS = 0x00000006,
        DISK = 0x00000007,
        DISK_FILE_SYSTEM = 0x00000008,
        FILE_SYSTEM = 0x00000009,
        INPORT_PORT = 0x0000000a,
        KEYBOARD = 0x0000000b,
        MAILSLOT = 0x0000000c,
        MIDI_IN = 0x0000000d,
        MIDI_OUT = 0x0000000e,
        MOUSE = 0x0000000f,
        MULTI_UNC_PROVIDER = 0x00000010,
        NAMED_PIPE = 0x00000011,
        NETWORK = 0x00000012,
        NETWORK_BROWSER = 0x00000013,
        NETWORK_FILE_SYSTEM = 0x00000014,
        NULL = 0x00000015,
        PARALLEL_PORT = 0x00000016,
        PHYSICAL_NETCARD = 0x00000017,
        PRINTER = 0x00000018,
        SCANNER = 0x00000019,
        SERIAL_MOUSE_PORT = 0x0000001a,
        SERIAL_PORT = 0x0000001b,
        SCREEN = 0x0000001c,
        SOUND = 0x0000001d,
        STREAMS = 0x0000001e,
        TAPE = 0x0000001f,
        TAPE_FILE_SYSTEM = 0x00000020,
        TRANSPORT = 0x00000021,
        UNKNOWN = 0x00000022,
        VIDEO = 0x00000023,
        VIRTUAL_DISK = 0x00000024,
        WAVE_IN = 0x00000025,
        WAVE_OUT = 0x00000026,
        i8042_PORT = 0x00000027,
        NETWORK_REDIRECTOR = 0x00000028,
        BATTERY = 0x00000029,
        BUS_EXTENDER = 0x0000002a,
        MODEM = 0x0000002b,
        VDM = 0x0000002c,
        MASS_STORAGE = 0x0000002d,
        SMB = 0x0000002e,
        KS = 0x0000002f,
        CHANGER = 0x00000030,
        SMARTCARD = 0x00000031,
        ACPI = 0x00000032,
        DVD = 0x00000033,
        FULLSCREEN_VIDEO = 0x00000034,
        DFS_FILE_SYSTEM = 0x00000035,
        DFS_VOLUME = 0x00000036,
        SERENUM = 0x00000037,
        TERMSRV = 0x00000038,
        KSEC = 0x00000039,
        FIPS = 0x0000003A,
        INFINIBAND = 0x0000003B,
        VMBUS = 0x0000003E,
        CRYPT_PROVIDER = 0x0000003F,
        WPD = 0x00000040,
        BLUETOOTH = 0x00000041,
        MT_COMPOSITE = 0x00000042,
        MT_TRANSPORT = 0x00000043,
        BIOMETRIC = 0x00000044,
        PMI = 0x00000045,
        EHSTOR = 0x00000046,
        DEVAPI = 0x00000047,
        GPIO = 0x00000048,
        USBEX = 0x00000049,
        CONSOLE = 0x00000050,
        NFP = 0x00000051,
        SYSENV = 0x00000052,
        VIRTUAL_BLOCK = 0x00000053,
        POINT_OF_SERVICE = 0x00000054,
        STORAGE_REPLICATION = 0x00000055,
        TRUST_ENV = 0x00000056,
        UCM = 0x00000057,
        UCMTCPCI = 0x00000058,
        PERSISTENT_MEMORY = 0x00000059,
        NVDIMM = 0x0000005a,
        HOLOGRAPHIC = 0x0000005b,
        SDFXHCI = 0x0000005c,
        UCMUCSI = 0x0000005d,
    }

    public static class Utils
    {
    }
}
