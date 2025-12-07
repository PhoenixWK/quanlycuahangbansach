using System;

namespace BookStoreDTO
{
    public class TheLoaiDTO
    {
        public int MaTL { get; set; }
        public string TenTL { get; set; } = string.Empty;

        // Constructor without parameters
        public TheLoaiDTO()
        {
        }

        // Constructor with parameters
        public TheLoaiDTO(int maTL, string tenTL)
        {
            MaTL = maTL;
            TenTL = tenTL;
        }

        // Constructor for creating new category (without ID)
        public TheLoaiDTO(string tenTL)
        {
            TenTL = tenTL;
        }

        public override string ToString()
        {
            return TenTL;
        }

        public override bool Equals(object? obj)
        {
            if (obj is TheLoaiDTO other)
            {
                return MaTL == other.MaTL;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return MaTL.GetHashCode();
        }
    }
}