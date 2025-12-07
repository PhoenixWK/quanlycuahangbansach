using System;

namespace BookStoreDTO
{
    public class TacGiaDTO
    {
        public int MaTG { get; set; }
        public string TenTG { get; set; } = string.Empty;

        // Constructor without parameters
        public TacGiaDTO()
        {
        }

        // Constructor with parameters
        public TacGiaDTO(int maTG, string tenTG)
        {
            MaTG = maTG;
            TenTG = tenTG;
        }

        // Constructor for creating new author (without ID)
        public TacGiaDTO(string tenTG)
        {
            TenTG = tenTG;
        }

        public override string ToString()
        {
            return TenTG;
        }

        public override bool Equals(object? obj)
        {
            if (obj is TacGiaDTO other)
            {
                return MaTG == other.MaTG;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return MaTG.GetHashCode();
        }
    }
}