// Daftar pilihan (Enums)
public enum FoodType { Utama, Lauk }
public enum TipeGizi { Stamina, Konsentrasi, Netral } 
public enum ProfilRasa { Gurih, Manis, Sehat, Pedas } // Saya tambahkan Pedas untuk variasi

[System.Serializable]
public class FoodData
{
    public string id;
    public string foodName;
    public FoodType type;
    public TipeGizi tipeGizi;
    public ProfilRasa profilRasa;
    public float nutrition;
    public float baseAppeal;
    public string description;
}