using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using NUnit.Framework;
using Mono.Security.Interface;

namespace MonoTests
{
	[TestFixture]
	public class MartinTest
	{
		static public byte[] farscape_nopwd_pfx = { 0x30, 0x82, 0x06, 0xA3, 0x02, 0x01, 0x03, 0x30, 0x82, 0x06, 0x63, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x07, 0x01, 0xA0, 0x82, 0x06, 0x54, 0x04, 0x82, 0x06, 0x50, 0x30, 0x82, 0x06, 0x4C, 0x30, 0x82, 0x03, 0x8D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x07, 0x01, 0xA0, 0x82, 0x03, 0x7E, 0x04, 0x82, 0x03, 0x7A, 0x30, 0x82, 0x03, 0x76, 0x30, 0x82, 0x03, 0x72, 0x06, 0x0B, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x0C, 0x0A, 0x01, 0x02, 0xA0, 0x82, 0x02, 0xB6, 0x30, 0x82, 0x02, 0xB2, 0x30, 0x1C, 0x06, 0x0A, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x0C, 0x01, 0x03, 0x30, 
			0x0E, 0x04, 0x08, 0x31, 0xB9, 0x22, 0x7A, 0x73, 0xB6, 0x67, 0x3E, 0x02, 0x02, 0x07, 0xD0, 0x04, 0x82, 0x02, 0x90, 0x05, 0x3F, 0x9B, 0x6F, 0x4D, 0xE2, 0x97, 0xC0, 0x71, 0x61, 0xDC, 0x39, 0x33, 0x9B, 0x45, 0x36, 0xD1, 0xC2, 0xC1, 0x2E, 0xE3, 0x22, 0x88, 0xE2, 0x54, 0x18, 0xE8, 0xC9, 0x0E, 0xA7, 0xBB, 0x1B, 0xC6, 0xC8, 0x32, 0xD9, 0x47, 0x64, 0x40, 0xC2, 0x40, 0xDC, 0x34, 0xB5, 0x34, 0x5D, 0x8A, 0x56, 0xD9, 0xF6, 0x0A, 0x03, 0x93, 0x5D, 0xE5, 0x04, 0xDC, 0x5B, 0xBA, 0x49, 0x22, 0x0A, 0x51, 0x33, 0xFF, 0xF0, 0xAF, 0x5D, 0x1F, 0x97, 0x6A, 0x11, 0x1C, 0x6B, 0x1A, 0xCF, 0x2E, 0x41, 0xA1, 0xD0, 0x31, 
			0xC2, 0x2D, 0xDD, 0x83, 0xAA, 0x21, 0x0C, 0x0E, 0x78, 0xEE, 0x9C, 0x25, 0x74, 0xC5, 0x4F, 0xE4, 0x94, 0x84, 0xA8, 0xD9, 0x2F, 0x96, 0xF5, 0x06, 0x05, 0xAE, 0x99, 0xBF, 0x8B, 0xD6, 0x67, 0x5E, 0xCB, 0x61, 0x03, 0xCC, 0x5A, 0x5F, 0xAB, 0x82, 0x55, 0xB1, 0x8D, 0xCD, 0xFE, 0x1C, 0x25, 0x48, 0xA7, 0x1D, 0xFF, 0x2E, 0xC0, 0x23, 0x80, 0xF7, 0xE4, 0x22, 0x68, 0x07, 0xFF, 0x58, 0xA5, 0xAA, 0x71, 0x7A, 0xAB, 0x48, 0x2D, 0xE6, 0xDF, 0xB5, 0x3C, 0x90, 0x15, 0xE3, 0x55, 0x4A, 0xB4, 0x37, 0xFE, 0x7F, 0xE1, 0x5B, 0x0C, 0xF1, 0x01, 0x4C, 0x60, 0x2F, 0x6F, 0x59, 0x09, 0x2B, 0x96, 0xDC, 0xE2, 0x2C, 0xF0, 0xB9, 
			0xF3, 0x3E, 0x46, 0x5B, 0x68, 0xA9, 0xBB, 0x42, 0x8B, 0xAB, 0xA9, 0x68, 0x56, 0xF9, 0xB2, 0x2E, 0x93, 0xDD, 0xE9, 0xBB, 0x70, 0x9E, 0x2E, 0x48, 0xB9, 0xDB, 0x1C, 0x95, 0x0F, 0x67, 0xD4, 0x13, 0x02, 0x62, 0xE0, 0xFA, 0x18, 0x48, 0xAE, 0x31, 0xB6, 0x1F, 0x68, 0x7D, 0xB2, 0x16, 0x61, 0xCD, 0x04, 0x91, 0x50, 0xBF, 0x35, 0xBF, 0x76, 0xA3, 0x5B, 0x76, 0xFE, 0x3F, 0xAB, 0xB2, 0x59, 0x8B, 0xD0, 0xB7, 0xC6, 0x36, 0x0E, 0x2C, 0x31, 0x48, 0xFB, 0x69, 0x6F, 0x90, 0x37, 0x3F, 0xE1, 0x53, 0x36, 0x5A, 0x60, 0x53, 0x93, 0x46, 0xC4, 0x31, 0x92, 0x3B, 0x11, 0x9F, 0x67, 0xC3, 0xD0, 0x2E, 0x9F, 0x7D, 0xA8, 0xBE, 
			0xA3, 0xB2, 0xCF, 0x60, 0xA3, 0xCE, 0x9F, 0x4B, 0x72, 0xCD, 0x44, 0x26, 0x4C, 0x66, 0xF8, 0x75, 0x80, 0xFC, 0x23, 0xBC, 0xA1, 0x3A, 0xCA, 0xC9, 0xE7, 0x50, 0xA3, 0x79, 0x21, 0x2B, 0x2D, 0x09, 0x8C, 0x45, 0x89, 0xB6, 0xAF, 0x66, 0x3E, 0xF7, 0xFD, 0xA5, 0x69, 0x96, 0xB4, 0x65, 0xB5, 0xFE, 0x35, 0x1F, 0x80, 0xA7, 0x41, 0x90, 0xBA, 0x92, 0x8D, 0x3B, 0xC0, 0x37, 0xDE, 0x95, 0xA8, 0x0D, 0xF1, 0x1A, 0x9F, 0xD2, 0x70, 0xED, 0x38, 0x1E, 0xA2, 0xF1, 0x2B, 0x63, 0x62, 0xC5, 0xAE, 0x5D, 0x0F, 0xFC, 0x80, 0xFA, 0x0E, 0xE4, 0xE7, 0x6C, 0x62, 0x3B, 0x19, 0xBB, 0xA8, 0xE5, 0x1D, 0x3E, 0x06, 0x30, 0x0B, 0xE1, 
			0xCF, 0xB6, 0xB4, 0x87, 0x96, 0xA2, 0x5E, 0xF8, 0x0F, 0x13, 0xAE, 0x04, 0xAF, 0xB2, 0x6C, 0x9E, 0xA0, 0x28, 0x1C, 0x46, 0xE5, 0xA8, 0x25, 0x62, 0x51, 0x95, 0xB0, 0x70, 0x60, 0xB6, 0xD9, 0xBB, 0xE3, 0xD1, 0xF0, 0x1D, 0x25, 0xBD, 0x93, 0x5E, 0xB6, 0x47, 0x50, 0xCD, 0x77, 0x7A, 0xFF, 0xC5, 0xFF, 0x4A, 0x7A, 0x9A, 0x27, 0x22, 0xEB, 0x7C, 0x12, 0xE5, 0x59, 0x1F, 0x60, 0xEA, 0xC3, 0x93, 0x4D, 0x28, 0x49, 0x2D, 0xF9, 0xC0, 0x13, 0x12, 0x89, 0x96, 0xED, 0x78, 0xB0, 0x1C, 0x82, 0xDE, 0xEE, 0x40, 0xDE, 0x68, 0x2B, 0x45, 0x16, 0xBE, 0xBF, 0xD5, 0x85, 0x6A, 0xDB, 0xD9, 0x1E, 0xEE, 0xFA, 0x6C, 0x95, 0x19, 
			0xF3, 0x76, 0x61, 0x72, 0x21, 0x69, 0x77, 0x18, 0x2C, 0xFA, 0x99, 0x7A, 0xD7, 0x58, 0xC4, 0xD6, 0x1D, 0x8B, 0xE8, 0x0B, 0xEC, 0x0F, 0x0F, 0xFA, 0xCE, 0xE2, 0x6F, 0xB1, 0xF5, 0x4F, 0xC3, 0xF7, 0x4A, 0xE4, 0x79, 0xB0, 0xFC, 0x62, 0x88, 0xC0, 0x49, 0xEC, 0xDB, 0xC8, 0xCD, 0xBE, 0x25, 0x00, 0x68, 0xB6, 0x5E, 0x89, 0x78, 0xE6, 0x92, 0xA5, 0x5D, 0x55, 0xA7, 0xAD, 0xFF, 0x3D, 0xC7, 0xF9, 0x95, 0x8D, 0xCF, 0x6E, 0x37, 0x1D, 0x79, 0x74, 0xE1, 0xDE, 0x22, 0x07, 0x6B, 0xE6, 0xB7, 0x7D, 0xD1, 0x0F, 0xB6, 0xA4, 0x3F, 0x0F, 0x31, 0x81, 0x09, 0xAD, 0xFD, 0x5F, 0xA4, 0xF4, 0x8F, 0x3C, 0x02, 0xB8, 0xB0, 0x04, 
			0x70, 0x44, 0x2C, 0x73, 0x42, 0xEE, 0xFF, 0xBA, 0x45, 0x50, 0xC0, 0x95, 0xFF, 0x62, 0x14, 0x91, 0x23, 0xF2, 0x8A, 0x65, 0x40, 0x20, 0xEB, 0x4B, 0x7B, 0x66, 0xF2, 0xC2, 0xC8, 0xD7, 0x16, 0x93, 0x0A, 0xBD, 0x5C, 0xCC, 0x11, 0x38, 0xEA, 0x90, 0x9C, 0x37, 0xDA, 0xB2, 0x80, 0xBF, 0x5C, 0x41, 0xC8, 0x3B, 0x16, 0x81, 0x83, 0xF7, 0xE4, 0x16, 0x12, 0x6C, 0x5F, 0x05, 0xBE, 0x2B, 0x04, 0x62, 0x36, 0x13, 0x8F, 0xF1, 0xC2, 0x5A, 0xCB, 0xFB, 0x26, 0x04, 0xE0, 0x31, 0x81, 0xA8, 0x30, 0x0D, 0x06, 0x09, 0x2B, 0x06, 0x01, 0x04, 0x01, 0x82, 0x37, 0x11, 0x02, 0x31, 0x00, 0x30, 0x13, 0x06, 0x09, 0x2A, 0x86, 0x48, 
			0x86, 0xF7, 0x0D, 0x01, 0x09, 0x15, 0x31, 0x06, 0x04, 0x04, 0x01, 0x00, 0x00, 0x00, 0x30, 0x17, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x09, 0x14, 0x31, 0x0A, 0x1E, 0x08, 0x00, 0x4D, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x6F, 0x30, 0x69, 0x06, 0x09, 0x2B, 0x06, 0x01, 0x04, 0x01, 0x82, 0x37, 0x11, 0x01, 0x31, 0x5C, 0x1E, 0x5A, 0x00, 0x4D, 0x00, 0x69, 0x00, 0x63, 0x00, 0x72, 0x00, 0x6F, 0x00, 0x73, 0x00, 0x6F, 0x00, 0x66, 0x00, 0x74, 0x00, 0x20, 0x00, 0x52, 0x00, 0x53, 0x00, 0x41, 0x00, 0x20, 0x00, 0x53, 0x00, 0x43, 0x00, 0x68, 0x00, 0x61, 0x00, 0x6E, 0x00, 0x6E, 0x00, 0x65, 0x00, 0x6C, 
			0x00, 0x20, 0x00, 0x43, 0x00, 0x72, 0x00, 0x79, 0x00, 0x70, 0x00, 0x74, 0x00, 0x6F, 0x00, 0x67, 0x00, 0x72, 0x00, 0x61, 0x00, 0x70, 0x00, 0x68, 0x00, 0x69, 0x00, 0x63, 0x00, 0x20, 0x00, 0x50, 0x00, 0x72, 0x00, 0x6F, 0x00, 0x76, 0x00, 0x69, 0x00, 0x64, 0x00, 0x65, 0x00, 0x72, 0x30, 0x82, 0x02, 0xB7, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x07, 0x06, 0xA0, 0x82, 0x02, 0xA8, 0x30, 0x82, 0x02, 0xA4, 0x02, 0x01, 0x00, 0x30, 0x82, 0x02, 0x9D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x07, 0x01, 0x30, 0x1C, 0x06, 0x0A, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x0C, 0x01, 
			0x06, 0x30, 0x0E, 0x04, 0x08, 0x37, 0xAE, 0x94, 0x2A, 0x4C, 0x78, 0xA2, 0x9A, 0x02, 0x02, 0x07, 0xD0, 0x80, 0x82, 0x02, 0x70, 0x49, 0xB9, 0xA3, 0x6E, 0xC7, 0x96, 0xCF, 0x92, 0x12, 0x43, 0x69, 0x57, 0xAD, 0x4B, 0x88, 0xA8, 0x3F, 0xEA, 0x25, 0xB6, 0xE4, 0x16, 0x74, 0x4E, 0xF5, 0xF8, 0xF2, 0xEC, 0xC0, 0xB7, 0xC2, 0x6A, 0x6E, 0xC0, 0x67, 0x5A, 0x5D, 0xFE, 0x0A, 0x7C, 0xBD, 0x06, 0xFF, 0x2F, 0x34, 0xFD, 0xE4, 0x06, 0x70, 0x23, 0xA3, 0x28, 0x27, 0xCA, 0x91, 0xD0, 0xC7, 0xA1, 0x08, 0x4F, 0x78, 0x0E, 0x89, 0xED, 0x29, 0x8F, 0xD6, 0x8E, 0x1C, 0xE0, 0x30, 0x08, 0x77, 0xA0, 0x3F, 0x18, 0xF1, 0x81, 0xD5, 
			0x73, 0xD7, 0x1A, 0xCA, 0xD4, 0x6D, 0x56, 0x7D, 0xFD, 0x30, 0xB5, 0xA0, 0x5D, 0x59, 0x82, 0xB9, 0xF7, 0x02, 0x19, 0x83, 0x68, 0x19, 0x08, 0x5E, 0x26, 0xCF, 0x06, 0xFA, 0xA0, 0xB4, 0x85, 0x95, 0x10, 0x6F, 0x91, 0x82, 0x89, 0xE8, 0x46, 0xEE, 0x51, 0xEB, 0x2A, 0x45, 0xAC, 0x93, 0x87, 0x48, 0x8C, 0xB6, 0x02, 0xB4, 0x4D, 0xC6, 0xFC, 0x51, 0x4C, 0x75, 0x9D, 0x5A, 0xE7, 0x46, 0x5B, 0x0A, 0x9D, 0x75, 0xA3, 0x0C, 0xB2, 0x54, 0x2A, 0x3E, 0x3A, 0x25, 0xA3, 0x75, 0x66, 0x52, 0x61, 0x7A, 0x78, 0xED, 0xDD, 0x7E, 0xF2, 0x4A, 0xA6, 0xB6, 0x3D, 0xEA, 0x62, 0xE4, 0x68, 0x95, 0x74, 0x3D, 0x45, 0xC1, 0x6E, 0x6B, 
			0xB6, 0x6E, 0x8F, 0x97, 0x39, 0xB5, 0x4F, 0xAA, 0x8E, 0xBB, 0x55, 0x10, 0x19, 0xCB, 0x66, 0xA0, 0xBF, 0xAE, 0x8B, 0xE6, 0xBC, 0x92, 0x8D, 0x2D, 0xC1, 0x83, 0x87, 0x53, 0x81, 0x32, 0x3B, 0x8E, 0x80, 0x76, 0xF9, 0xDE, 0x60, 0x8F, 0x99, 0x02, 0x4F, 0x97, 0x73, 0x3D, 0xE3, 0xC7, 0xBA, 0xBD, 0x4C, 0x3F, 0x8A, 0x9B, 0xE3, 0xFE, 0x24, 0xC3, 0x3E, 0xDE, 0x02, 0x0F, 0x46, 0x84, 0x79, 0xDF, 0x5E, 0xC9, 0xA3, 0x7C, 0x58, 0x62, 0xFC, 0x1D, 0x9F, 0x5E, 0x9A, 0xDB, 0x3C, 0x45, 0x96, 0x91, 0xFD, 0xD9, 0xD0, 0xE7, 0x7F, 0x72, 0xBA, 0x2D, 0xC5, 0x3A, 0x54, 0xBC, 0xA0, 0xAE, 0xAA, 0xFF, 0xE9, 0x18, 0x0C, 0x1B, 
			0x9A, 0xD4, 0xDA, 0x82, 0xBF, 0x51, 0x23, 0xB3, 0x6E, 0xEF, 0xDB, 0x85, 0xE5, 0xBF, 0x02, 0xCC, 0xFB, 0x79, 0xA6, 0x45, 0x86, 0xDC, 0xDF, 0xF0, 0x2C, 0x15, 0x0B, 0xD1, 0xE5, 0x80, 0xBB, 0x3F, 0x65, 0x94, 0xE5, 0xAB, 0x76, 0xE4, 0xA5, 0x92, 0x7D, 0x0E, 0x8C, 0xC0, 0x92, 0x83, 0x40, 0x9D, 0x2F, 0xBD, 0x30, 0xE1, 0x7B, 0xB5, 0x91, 0xB2, 0x5E, 0xD9, 0xC6, 0xB7, 0xA4, 0x30, 0x06, 0x18, 0xED, 0x33, 0x95, 0x7B, 0xA6, 0xE3, 0xE5, 0xC0, 0x4B, 0xF5, 0x0B, 0x6A, 0x3A, 0xF5, 0xAC, 0x77, 0x22, 0xC0, 0x84, 0x3C, 0x5B, 0xE5, 0x55, 0xD5, 0xDC, 0x7E, 0xFE, 0x08, 0x02, 0x37, 0x69, 0x52, 0xB8, 0x44, 0x29, 0x16, 
			0xB5, 0xE8, 0x8A, 0xA4, 0xAC, 0x24, 0x58, 0xC3, 0x53, 0xAC, 0x37, 0xE2, 0xD4, 0x0F, 0x21, 0xC1, 0x54, 0x62, 0x28, 0xCA, 0xA3, 0x8C, 0x01, 0x26, 0x97, 0xFF, 0xAD, 0x0E, 0x5F, 0xB1, 0x86, 0x96, 0xD1, 0xFA, 0xE5, 0x9F, 0x38, 0x42, 0x4D, 0x32, 0xEB, 0xC8, 0x4B, 0x4A, 0x01, 0x91, 0x5C, 0xCE, 0xC8, 0x89, 0x0A, 0x7C, 0x32, 0x6D, 0x08, 0x3E, 0x7D, 0xB0, 0x3D, 0x16, 0x99, 0x52, 0xB0, 0xE0, 0xBE, 0xFF, 0x42, 0x61, 0xC3, 0x56, 0xE1, 0x9A, 0xA3, 0xFB, 0x72, 0xBB, 0x3B, 0x4C, 0xA3, 0xFC, 0x5E, 0xFE, 0xC7, 0xF2, 0xBB, 0x17, 0x96, 0x00, 0xB6, 0x02, 0xD1, 0x58, 0xF0, 0xDA, 0x63, 0xD2, 0x4C, 0x91, 0xDF, 0xFA, 
			0xB5, 0xAF, 0x1E, 0xDA, 0xD7, 0x02, 0x85, 0xFE, 0x80, 0x94, 0x77, 0x92, 0x84, 0x9A, 0x2F, 0x1C, 0xC2, 0x71, 0xA7, 0x3F, 0xFA, 0x00, 0xFC, 0x7E, 0x4B, 0xE2, 0xD2, 0x7B, 0xC8, 0xB9, 0x26, 0xEC, 0xD4, 0x7A, 0x3D, 0x6F, 0x89, 0xB4, 0x22, 0x2F, 0xE9, 0x41, 0xA9, 0x97, 0x8C, 0x76, 0xCE, 0xCD, 0xA6, 0x94, 0xA9, 0x1D, 0x25, 0x7C, 0x4D, 0xCF, 0x2E, 0x51, 0x59, 0xE9, 0xE3, 0xDB, 0x84, 0x28, 0x2E, 0x31, 0x24, 0xF9, 0xA7, 0xC0, 0xA7, 0x77, 0xD0, 0xB5, 0x19, 0x1C, 0xC9, 0x22, 0x28, 0x94, 0x39, 0xF5, 0xC3, 0xAA, 0x78, 0x3A, 0xE6, 0x1D, 0xB3, 0xCA, 0x95, 0x7F, 0x7D, 0xBD, 0xFA, 0x7F, 0xCD, 0x09, 0xA5, 0x77, 
			0x8E, 0xC8, 0xEB, 0x03, 0x26, 0xAF, 0x38, 0x5A, 0x9A, 0xFB, 0xDC, 0x90, 0xBD, 0xD7, 0x46, 0xA7, 0xB4, 0x71, 0x8F, 0xF7, 0x66, 0x4A, 0x07, 0x66, 0xE4, 0xD7, 0x3E, 0xC4, 0xD4, 0x2B, 0x15, 0x1F, 0xC8, 0x9C, 0x3A, 0x47, 0x5E, 0x6F, 0x84, 0xE3, 0x02, 0x62, 0x05, 0x86, 0x63, 0x30, 0x37, 0x30, 0x1F, 0x30, 0x07, 0x06, 0x05, 0x2B, 0x0E, 0x03, 0x02, 0x1A, 0x04, 0x14, 0x62, 0x54, 0xAE, 0x53, 0x8C, 0x33, 0xEC, 0x3E, 0x2D, 0x73, 0xE6, 0xEB, 0x9A, 0xDD, 0x31, 0xEE, 0x06, 0x83, 0x4B, 0xBA, 0x04, 0x14, 0x60, 0x9B, 0x73, 0xDD, 0x3F, 0x8F, 0x2E, 0x52, 0x1C, 0x4C, 0xB9, 0x8E, 0x7A, 0xC0, 0xCD, 0x52, 0xB4, 0xBA, 
			0xBD, 0x8C };

		[SetUp]
		public void TestProvider ()
		{
			MonoTlsProviderFactory.Initialize ();
			var provider = MonoTlsProviderFactory.GetProvider ();
			Console.Error.WriteLine ("PROVIDER: {0}", provider);
		}

		[Test]
		public void TestCertificate ()
		{
			var cert = new X509Certificate2 (farscape_nopwd_pfx);
			Console.Error.WriteLine ("TEST: {0}", cert);
		}
	}
}
