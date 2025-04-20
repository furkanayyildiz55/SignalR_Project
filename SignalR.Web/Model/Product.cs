namespace SignalR.Web.Model
{
    //Record yapıspı, C# 9.0 ile gelen bir özellik olup, veri taşıma nesneleri (DTO) oluşturmak için kullanılır.
    //Record yapısı, sınıflara benzer şekilde tanımlanır, ancak daha az kod yazarak veri taşıma nesneleri oluşturmanıza olanak tanır.

    public record Product(int Id, string Name, decimal Price);
}
