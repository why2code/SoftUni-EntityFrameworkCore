using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ProductShop;
using ProductShop.Data;
using ProductShop.Models;

[TestFixture]
public class Test_006_000_001
{
    private IServiceProvider serviceProvider;

    private static readonly Assembly CurrentAssembly = typeof(StartUp).Assembly;

    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

        this.serviceProvider = ConfigureServices<ProductShopContext>(Guid.NewGuid().ToString());
    }

    [Test]
    public void ExportSoldProductsZeroTests()
    {
        var context = this.serviceProvider.GetService<ProductShopContext>();

        SeedDatabase(context);

        var expectedXml =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?><Users><User><firstName>Almire</firstName><lastName>Ainslee</lastName><soldProducts><Product><name>olio activ mouthwash</name><price>206.06</price></Product><Product><name>Acnezzol Base</name><price>710.6</price></Product><Product><name>ENALAPRIL MALEATE</name><price>210.42</price></Product></soldProducts></User><User><firstName>Etta</firstName><lastName>Arnaudi</lastName><soldProducts><Product><name>pain relief</name><price>938.23</price></Product><Product><name>Ketorolac Tromethamine</name><price>608.18</price></Product><Product><name>Agaricus Equisetum Special Order</name><price>585.93</price></Product><Product><name>Flumazenil</name><price>1151.37</price></Product><Product><name>Ringers</name><price>1054.37</price></Product><Product><name>Yellow Jacket hymenoptera venom Venomil Diagnostic</name><price>23.58</price></Product></soldProducts></User><User><firstName>Jania</firstName><lastName>Atger</lastName><soldProducts><Product><name>Carbon Dioxide Oxygen Mixture</name><price>95.49</price></Product><Product><name>Allopurinol</name><price>518.5</price></Product><Product><name>ROPINIROLE HYDROCHLORIDE</name><price>266.44</price></Product><Product><name>Almond</name><price>367.32</price></Product></soldProducts></User><User><firstName>Duky</firstName><lastName>Bowller</lastName><soldProducts><Product><name>Fluoxetine</name><price>385.37</price></Product><Product><name>Extra Strength Pain Reliever PM</name><price>542.72</price></Product><Product><name>Propranolol Hydrochloride</name><price>546.95</price></Product><Product><name>CARBIDOPA AND LEVODOPA</name><price>441.64</price></Product><Product><name>Peter Island Continous sunscreen kids</name><price>471.3</price></Product><Product><name>CEDAX</name><price>342.86</price></Product></soldProducts></User><User><firstName>Clemmy</firstName><lastName>Bremmer</lastName><soldProducts><Product><name>Cefadroxil</name><price>1302.2</price></Product><Product><name>Fosphenytoin Sodium</name><price>1334.06</price></Product></soldProducts></User></Users>";

        var actualXml = StartUp.GetSoldProducts(context);
        var actualOutput = XDocument.Parse(actualXml);
        var expectedOutput = XDocument.Parse(expectedXml);

        var expected = expectedOutput.ToString(SaveOptions.DisableFormatting);
        var actual = actualOutput.ToString(SaveOptions.DisableFormatting);

        Assert.That(actual, Is.EqualTo(expected).NoClip,
            $"{nameof(StartUp.GetSoldProducts)} output is incorrect!");
    }

    private static void SeedDatabase(ProductShopContext context)
    {
        var datasetsJson =
            "{\"User\":[{\"Id\":1,\"FirstName\":\"Chrissy\",\"LastName\":\"Falconbridge\",\"Age\":50},{\"Id\":2,\"FirstName\":\"Wendel\",\"LastName\":\"Stannering\",\"Age\":34},{\"Id\":3,\"FirstName\":\"Jacquelin\",\"LastName\":\"Fransoni\",\"Age\":22},{\"Id\":4,\"FirstName\":\"Kevyn\",\"LastName\":\"Priestley\",\"Age\":22},{\"Id\":5,\"FirstName\":\"Mitchell\",\"LastName\":\"Worboys\",\"Age\":39},{\"Id\":6,\"FirstName\":\"Kayla\",\"LastName\":\"Middlebrook\",\"Age\":22},{\"Id\":7,\"FirstName\":\"Clementius\",\"LastName\":\"Jedrych\",\"Age\":33},{\"Id\":8,\"FirstName\":\"Vivie\",\"LastName\":\"Tyrwhitt\",\"Age\":29},{\"Id\":9,\"FirstName\":\"Purcell\",\"LastName\":\"Prewett\",\"Age\":47},{\"Id\":10,\"FirstName\":\"Arel\",\"LastName\":\"Weatherburn\",\"Age\":25},{\"Id\":11,\"FirstName\":\"Dannie\",\"LastName\":\"Camellini\",\"Age\":47},{\"Id\":12,\"FirstName\":\"Sal\",\"LastName\":\"Loseby\",\"Age\":46},{\"Id\":13,\"FirstName\":\"Hartwell\",\"LastName\":\"Gietz\",\"Age\":42},{\"Id\":14,\"FirstName\":\"Nathanil\",\"LastName\":\"Mence\",\"Age\":35},{\"Id\":15,\"FirstName\":\"Leeland\",\"LastName\":\"Nurdin\",\"Age\":28},{\"Id\":16,\"FirstName\":\"Cathee\",\"LastName\":\"Rallings\",\"Age\":33},{\"Id\":17,\"FirstName\":\"Margi\",\"LastName\":\"Ellerton\",\"Age\":23},{\"Id\":18,\"FirstName\":\"Sallyann\",\"LastName\":\"Skitch\",\"Age\":28},{\"Id\":19,\"FirstName\":\"Vinni\",\"LastName\":\"Capaldo\",\"Age\":43},{\"Id\":20,\"FirstName\":\"Fanny\",\"LastName\":\"Millmoe\",\"Age\":21},{\"Id\":21,\"FirstName\":\"Omar\",\"LastName\":\"Jaffray\",\"Age\":26},{\"Id\":22,\"FirstName\":\"Etta\",\"LastName\":\"Arnaudi\",\"Age\":32},{\"Id\":23,\"FirstName\":\"Saxe\",\"LastName\":\"Ivory\",\"Age\":33},{\"Id\":24,\"FirstName\":\"Hermine\",\"LastName\":\"Hughland\",\"Age\":36},{\"Id\":25,\"FirstName\":\"Wallas\",\"LastName\":\"Duffyn\",\"Age\":44},{\"Id\":26,\"FirstName\":\"Dominik\",\"LastName\":\"Humpatch\",\"Age\":22},{\"Id\":27,\"FirstName\":\"Duky\",\"LastName\":\"Bowller\",\"Age\":30},{\"Id\":28,\"FirstName\":\"Fianna\",\"LastName\":\"Lathom\",\"Age\":48},{\"Id\":29,\"FirstName\":\"Stephine\",\"LastName\":\"Trubshawe\",\"Age\":36},{\"Id\":30,\"FirstName\":\"Desiri\",\"LastName\":\"Sumption\",\"Age\":35},{\"Id\":31,\"FirstName\":\"Rosy\",\"LastName\":\"Cromley\",\"Age\":37},{\"Id\":32,\"FirstName\":\"Brendin\",\"LastName\":\"Predohl\",\"Age\":38},{\"Id\":33,\"FirstName\":\"Jania\",\"LastName\":\"Atger\",\"Age\":21},{\"Id\":34,\"FirstName\":\"Kingsly\",\"LastName\":\"Hosby\",\"Age\":47},{\"Id\":35,\"FirstName\":\"Hank\",\"LastName\":\"Drinnan\",\"Age\":31},{\"Id\":36,\"FirstName\":\"Bogey\",\"LastName\":\"Grinyov\",\"Age\":44},{\"Id\":37,\"FirstName\":\"Clemmy\",\"LastName\":\"Bremmer\",\"Age\":25},{\"Id\":38,\"FirstName\":\"Brig\",\"LastName\":\"Mullineux\",\"Age\":39},{\"Id\":39,\"FirstName\":\"Bernadette\",\"LastName\":\"Ensor\",\"Age\":25},{\"Id\":40,\"FirstName\":\"Ed\",\"LastName\":\"Saxon\",\"Age\":23},{\"Id\":41,\"FirstName\":\"Murdoch\",\"LastName\":\"Greensted\",\"Age\":34},{\"Id\":42,\"FirstName\":\"Osborn\",\"LastName\":\"McGettigan\",\"Age\":28},{\"Id\":43,\"FirstName\":\"Rouvin\",\"LastName\":\"Wilcockes\",\"Age\":22},{\"Id\":44,\"FirstName\":\"Devland\",\"LastName\":\"Vazquez\",\"Age\":27},{\"Id\":45,\"FirstName\":\"Penelope\",\"LastName\":\"Soldner\",\"Age\":32},{\"Id\":46,\"FirstName\":\"Vince\",\"LastName\":\"Hills\",\"Age\":44},{\"Id\":47,\"FirstName\":\"Guthrie\",\"LastName\":\"Hallyburton\",\"Age\":32},{\"Id\":48,\"FirstName\":\"Rabbi\",\"LastName\":\"Bunten\",\"Age\":39},{\"Id\":49,\"FirstName\":\"Lilly\",\"LastName\":\"Frend\",\"Age\":23},{\"Id\":50,\"FirstName\":\"Crystie\",\"LastName\":\"Feronet\",\"Age\":29},{\"Id\":51,\"FirstName\":\"Almire\",\"LastName\":\"Ainslee\",\"Age\":27},{\"Id\":52,\"FirstName\":\"Pier\",\"LastName\":\"Prosser\",\"Age\":48},{\"Id\":53,\"FirstName\":\"Seymour\",\"LastName\":\"Greatrex\",\"Age\":43},{\"Id\":54,\"FirstName\":\"Carine\",\"LastName\":\"Louthe\",\"Age\":41},{\"Id\":55,\"FirstName\":\"Emiline\",\"LastName\":\"Gingold\",\"Age\":44},{\"Id\":56,\"FirstName\":\"Letizia\",\"LastName\":\"Flukes\",\"Age\":49},{\"Id\":57,\"FirstName\":\"Crista\",\"LastName\":\"Byrde\",\"Age\":37},{\"Id\":58,\"FirstName\":\"Dale\",\"LastName\":\"Galbreath\",\"Age\":31},{\"Id\":59,\"FirstName\":\"Corina\",\"LastName\":\"Bonifas\",\"Age\":22},{\"Id\":60,\"FirstName\":\"Neron\",\"LastName\":\"O\'Keenan\",\"Age\":38},{\"Id\":61,\"FirstName\":\"Sly\",\"LastName\":\"Biaggetti\",\"Age\":49},{\"Id\":62,\"FirstName\":\"Lethia\",\"LastName\":\"Neumann\",\"Age\":31},{\"Id\":63,\"FirstName\":\"Bidget\",\"LastName\":\"Chritchlow\",\"Age\":28},{\"Id\":64,\"FirstName\":\"Ursola\",\"LastName\":\"McVeighty\",\"Age\":38},{\"Id\":65,\"FirstName\":\"Freddy\",\"LastName\":\"Duckit\",\"Age\":34},{\"Id\":66,\"FirstName\":\"Elysia\",\"LastName\":\"Fusedale\",\"Age\":35},{\"Id\":67,\"FirstName\":\"Shelby\",\"LastName\":\"Clampett\",\"Age\":31},{\"Id\":68,\"FirstName\":\"Tod\",\"LastName\":\"Staig\",\"Age\":48},{\"Id\":69,\"FirstName\":\"Karena\",\"LastName\":\"Aldersea\",\"Age\":45},{\"Id\":70,\"FirstName\":\"Burton\",\"LastName\":\"Dimsdale\",\"Age\":35},{\"Id\":71,\"FirstName\":\"Josepha\",\"LastName\":\"Virgoe\",\"Age\":50},{\"Id\":72,\"FirstName\":\"Etty\",\"LastName\":\"Haill\",\"Age\":31},{\"Id\":73,\"FirstName\":\"Claybourne\",\"LastName\":\"Gaber\",\"Age\":42},{\"Id\":74,\"FirstName\":\"Art\",\"LastName\":\"Bensusan\",\"Age\":22},{\"Id\":75,\"FirstName\":\"Corabella\",\"LastName\":\"Farryann\",\"Age\":45},{\"Id\":76,\"FirstName\":\"Addie\",\"LastName\":\"Leroy\",\"Age\":26},{\"Id\":77,\"FirstName\":\"Karoly\",\"LastName\":\"Willmer\",\"Age\":29},{\"Id\":78,\"FirstName\":\"Harmonie\",\"LastName\":\"Berford\",\"Age\":39},{\"Id\":79,\"FirstName\":\"Myrwyn\",\"LastName\":\"Kenworthy\",\"Age\":21},{\"Id\":80,\"FirstName\":\"Iseabal\",\"LastName\":\"Hallam\",\"Age\":22},{\"Id\":81,\"FirstName\":\"Ainslie\",\"LastName\":\"Skedgell\",\"Age\":27},{\"Id\":82,\"FirstName\":\"Dolf\",\"LastName\":\"Puller\",\"Age\":38},{\"Id\":83,\"FirstName\":\"Jannelle\",\"LastName\":\"Petrasch\",\"Age\":29},{\"Id\":84,\"FirstName\":\"Charissa\",\"LastName\":\"Braybrooks\",\"Age\":34},{\"Id\":85,\"FirstName\":\"Lanie\",\"LastName\":\"Patrone\",\"Age\":23},{\"Id\":86,\"FirstName\":\"Joan\",\"LastName\":\"Nendick\",\"Age\":31},{\"Id\":87,\"FirstName\":\"Lorne\",\"LastName\":\"Waison\",\"Age\":26},{\"Id\":88,\"FirstName\":\"Evaleen\",\"LastName\":\"Cund\",\"Age\":41},{\"Id\":89,\"FirstName\":\"Arden\",\"LastName\":\"Arboin\",\"Age\":46},{\"Id\":90,\"FirstName\":\"Les\",\"LastName\":\"Saph\",\"Age\":30},{\"Id\":91,\"FirstName\":\"Sonnie\",\"LastName\":\"de Quincey\",\"Age\":21},{\"Id\":92,\"FirstName\":\"Oralle\",\"LastName\":\"Diboll\",\"Age\":31},{\"Id\":93,\"FirstName\":\"Ophelie\",\"LastName\":\"Hakeworth\",\"Age\":34},{\"Id\":94,\"FirstName\":\"Fidel\",\"LastName\":\"Perrett\",\"Age\":26},{\"Id\":95,\"FirstName\":\"Edi\",\"LastName\":\"Bolzmann\",\"Age\":39},{\"Id\":96,\"FirstName\":\"Gerrie\",\"LastName\":\"Henrique\",\"Age\":39},{\"Id\":97,\"FirstName\":\"Leonardo\",\"LastName\":\"Rummery\",\"Age\":38},{\"Id\":98,\"FirstName\":\"Deeyn\",\"LastName\":\"Lawlan\",\"Age\":38},{\"Id\":99,\"FirstName\":\"Eba\",\"LastName\":\"Pleming\",\"Age\":42},{\"Id\":100,\"FirstName\":\"Phillie\",\"LastName\":\"Adamek\",\"Age\":20}],\"Category\":[{\"Id\":1,\"Name\":\"Drugs\"},{\"Id\":2,\"Name\":\"Adult\"},{\"Id\":3,\"Name\":\"Electronics\"},{\"Id\":4,\"Name\":\"Garden\"},{\"Id\":5,\"Name\":\"Weapons\"},{\"Id\":6,\"Name\":\"For Children\"},{\"Id\":7,\"Name\":\"Sports\"},{\"Id\":8,\"Name\":\"Fashion\"},{\"Id\":9,\"Name\":\"Autoparts\"},{\"Id\":10,\"Name\":\"Business\"},{\"Id\":11,\"Name\":\"Other\"}],\"Product\":[{\"Id\":1,\"Name\":\"Care One Hemorrhoidal\",\"Price\":932.18,\"SellerId\":25,\"BuyerId\":24},{\"Id\":2,\"Name\":\"Cefadroxil\",\"Price\":1302.2,\"SellerId\":37,\"BuyerId\":44},{\"Id\":3,\"Name\":\"Warfarin Sodium\",\"Price\":138.83,\"SellerId\":50,\"BuyerId\":18},{\"Id\":4,\"Name\":\"PAMO Kill Natural\",\"Price\":1181.06,\"SellerId\":52,\"BuyerId\":3},{\"Id\":5,\"Name\":\"Fluoxetine\",\"Price\":385.37,\"SellerId\":27,\"BuyerId\":44},{\"Id\":6,\"Name\":\"Gemcitabine\",\"Price\":594.79,\"SellerId\":42,\"BuyerId\":38},{\"Id\":7,\"Name\":\"Aspen\",\"Price\":1046.46,\"SellerId\":8,\"BuyerId\":46},{\"Id\":8,\"Name\":\"AIR\",\"Price\":581.69,\"SellerId\":44,\"BuyerId\":24},{\"Id\":9,\"Name\":\"AVANDAMET\",\"Price\":632.08,\"SellerId\":24,\"BuyerId\":51},{\"Id\":10,\"Name\":\"Fair Foundation SPF 15\",\"Price\":1394.24,\"SellerId\":16,\"BuyerId\":29},{\"Id\":11,\"Name\":\"Pleo Ginkgo\",\"Price\":613.65,\"SellerId\":11,\"BuyerId\":33},{\"Id\":12,\"Name\":\"Irbesartan and Hydrochlorothiazide\",\"Price\":308.3,\"SellerId\":28,\"BuyerId\":19},{\"Id\":13,\"Name\":\"IOPE SUPER VITAL\",\"Price\":824.68,\"SellerId\":2,\"BuyerId\":28},{\"Id\":14,\"Name\":\"Ibuprofen\",\"Price\":1088.04,\"SellerId\":44,\"BuyerId\":3},{\"Id\":15,\"Name\":\"PREMIER VALUE ALLERGY\",\"Price\":1127.61,\"SellerId\":9,\"BuyerId\":35},{\"Id\":16,\"Name\":\"Labetalol Hydrochloride\",\"Price\":1345.11,\"SellerId\":14,\"BuyerId\":5},{\"Id\":17,\"Name\":\"Prednisone\",\"Price\":1285.99,\"SellerId\":34,\"BuyerId\":55},{\"Id\":18,\"Name\":\"GProducting Pains\",\"Price\":1077.37,\"SellerId\":17,\"BuyerId\":7},{\"Id\":19,\"Name\":\"Extra Strength Pain Reliever PM\",\"Price\":542.72,\"SellerId\":27,\"BuyerId\":2},{\"Id\":20,\"Name\":\"Americaine\",\"Price\":1165.75,\"SellerId\":14,\"BuyerId\":25},{\"Id\":21,\"Name\":\"Echinacea Quartz Gum Support\",\"Price\":570.08,\"SellerId\":53,\"BuyerId\":24},{\"Id\":22,\"Name\":\"SUMATRIPTAN SUCCINATE\",\"Price\":1265.94,\"SellerId\":25,\"BuyerId\":2},{\"Id\":23,\"Name\":\"Topiramate\",\"Price\":578.77,\"SellerId\":39,\"BuyerId\":31},{\"Id\":24,\"Name\":\"Amitiza\",\"Price\":666.58,\"SellerId\":19,\"BuyerId\":25},{\"Id\":25,\"Name\":\"GEMCITABINE\",\"Price\":1468.11,\"SellerId\":36,\"BuyerId\":48},{\"Id\":26,\"Name\":\"Meloxicam\",\"Price\":809.18,\"SellerId\":1,\"BuyerId\":21},{\"Id\":27,\"Name\":\"MOIST MOISTURE SKIN TONER\",\"Price\":152.43,\"SellerId\":30,\"BuyerId\":28},{\"Id\":28,\"Name\":\"Strattera\",\"Price\":658.54,\"SellerId\":49,\"BuyerId\":27},{\"Id\":29,\"Name\":\"Aquafresh\",\"Price\":849.93,\"SellerId\":50,\"BuyerId\":13},{\"Id\":30,\"Name\":\"Nighttime Sleep Aid\",\"Price\":1217.11,\"SellerId\":29,\"BuyerId\":12},{\"Id\":31,\"Name\":\"Pollens - Weeds and Garden Plants, Nettle Urtica dioica\",\"Price\":782.77,\"SellerId\":19,\"BuyerId\":13},{\"Id\":32,\"Name\":\"Perrigo Benzoyl Peroxide\",\"Price\":873.24,\"SellerId\":19,\"BuyerId\":17},{\"Id\":33,\"Name\":\"ENBREL\",\"Price\":673.97,\"SellerId\":35,\"BuyerId\":26},{\"Id\":34,\"Name\":\"Propranolol Hydrochloride\",\"Price\":546.95,\"SellerId\":27,\"BuyerId\":3},{\"Id\":35,\"Name\":\"Allopurinol\",\"Price\":48.8,\"SellerId\":3,\"BuyerId\":30},{\"Id\":36,\"Name\":\"Trihexyphenidyl Hydrochloride\",\"Price\":64.88,\"SellerId\":54,\"BuyerId\":40},{\"Id\":37,\"Name\":\"Archangelica Eucalyptus\",\"Price\":1334.91,\"SellerId\":12,\"BuyerId\":4},{\"Id\":38,\"Name\":\"Effervescent Cold Relief\",\"Price\":1436.07,\"SellerId\":42,\"BuyerId\":46},{\"Id\":39,\"Name\":\"Allopurinol\",\"Price\":336.81,\"SellerId\":41,\"BuyerId\":9},{\"Id\":40,\"Name\":\"Parsley\",\"Price\":519.06,\"SellerId\":12,\"BuyerId\":32},{\"Id\":41,\"Name\":\"Protonix\",\"Price\":466.7,\"SellerId\":45,\"BuyerId\":13},{\"Id\":42,\"Name\":\"Pollens - Trees, Birch Mix\",\"Price\":1153.54,\"SellerId\":9,\"BuyerId\":36},{\"Id\":43,\"Name\":\"ropinirole hydrochloride\",\"Price\":103.58,\"SellerId\":35,\"BuyerId\":25},{\"Id\":44,\"Name\":\"olio activ mouthwash\",\"Price\":206.06,\"SellerId\":51,\"BuyerId\":3},{\"Id\":45,\"Name\":\"CARBIDOPA AND LEVODOPA\",\"Price\":441.64,\"SellerId\":27,\"BuyerId\":11},{\"Id\":46,\"Name\":\"Pollens - Weeds and Garden Plants, Scotch Broom Cytisus scoparius\",\"Price\":135.82,\"SellerId\":43,\"BuyerId\":20},{\"Id\":47,\"Name\":\"Azithromycin\",\"Price\":813.87,\"SellerId\":23,\"BuyerId\":25},{\"Id\":48,\"Name\":\"pain relief\",\"Price\":938.23,\"SellerId\":22,\"BuyerId\":46},{\"Id\":49,\"Name\":\"IOPE RETIGEN MOISTURE TWIN CAKE NO.21\",\"Price\":1257.71,\"SellerId\":16,\"BuyerId\":23},{\"Id\":50,\"Name\":\"Goats Milk\",\"Price\":298.53,\"SellerId\":1,\"BuyerId\":3},{\"Id\":51,\"Name\":\"Ondansetron\",\"Price\":1249.76,\"SellerId\":8,\"BuyerId\":26},{\"Id\":52,\"Name\":\"Pioglitazone\",\"Price\":306.56,\"SellerId\":23,\"BuyerId\":36},{\"Id\":53,\"Name\":\"Butalbital, Aspirin and Caffeine\",\"Price\":1010.98,\"SellerId\":43,\"BuyerId\":41},{\"Id\":54,\"Name\":\"Hydralazine Hydrochloride\",\"Price\":1309.72,\"SellerId\":45,\"BuyerId\":3},{\"Id\":55,\"Name\":\"Nevirapine\",\"Price\":1374.72,\"SellerId\":38,\"BuyerId\":31},{\"Id\":56,\"Name\":\"ESIKA\",\"Price\":879.37,\"SellerId\":16,\"BuyerId\":24},{\"Id\":57,\"Name\":\"Homeopathic Rheumatism\",\"Price\":967.08,\"SellerId\":38,\"BuyerId\":16},{\"Id\":58,\"Name\":\"Amitriptyline Hydrochloride\",\"Price\":1453.96,\"SellerId\":17,\"BuyerId\":51},{\"Id\":59,\"Name\":\"Ibuprofen\",\"Price\":1305.96,\"SellerId\":5,\"BuyerId\":10},{\"Id\":60,\"Name\":\"Laser Block 100\",\"Price\":1135.43,\"SellerId\":35,\"BuyerId\":7},{\"Id\":61,\"Name\":\"ANXIETY/STRESS RELIEF\",\"Price\":324.66,\"SellerId\":56,\"BuyerId\":43},{\"Id\":62,\"Name\":\"TYLENOL COLD MULTI-SYMPTOM DAYTIME\",\"Price\":1010.81,\"SellerId\":17,\"BuyerId\":26},{\"Id\":63,\"Name\":\"Naproxen\",\"Price\":807.22,\"SellerId\":7,\"BuyerId\":17},{\"Id\":64,\"Name\":\"Dover Aminophen\",\"Price\":192.07,\"SellerId\":15,\"BuyerId\":3},{\"Id\":65,\"Name\":\"DIPYRIDAMOLE\",\"Price\":1150.67,\"SellerId\":24,\"BuyerId\":1},{\"Id\":66,\"Name\":\"Etodolac\",\"Price\":1443.13,\"SellerId\":8,\"BuyerId\":44},{\"Id\":67,\"Name\":\"ziprasidone hydrochloride\",\"Price\":628.66,\"SellerId\":38,\"BuyerId\":5},{\"Id\":68,\"Name\":\"Treatment Set TS336667\",\"Price\":1466.47,\"SellerId\":52,\"BuyerId\":35},{\"Id\":69,\"Name\":\"Prostate\",\"Price\":716.05,\"SellerId\":9,\"BuyerId\":13},{\"Id\":70,\"Name\":\"Acid Reducer\",\"Price\":1443.51,\"SellerId\":25,\"BuyerId\":43},{\"Id\":71,\"Name\":\"leader pain reliever\",\"Price\":1179.79,\"SellerId\":46,\"BuyerId\":2},{\"Id\":72,\"Name\":\"allergy eye\",\"Price\":426.91,\"SellerId\":16,\"BuyerId\":1},{\"Id\":73,\"Name\":\"Metoprolol Tartrate\",\"Price\":1405.74,\"SellerId\":21,\"BuyerId\":49},{\"Id\":74,\"Name\":\"Glycopyrrolate\",\"Price\":1471.43,\"SellerId\":17,\"BuyerId\":40},{\"Id\":75,\"Name\":\"Air\",\"Price\":331.53,\"SellerId\":45,\"BuyerId\":38},{\"Id\":76,\"Name\":\"Triamterene and Hydrochlorothiazide\",\"Price\":1416.59,\"SellerId\":2,\"BuyerId\":32},{\"Id\":77,\"Name\":\"Baza Antifungal\",\"Price\":1162.34,\"SellerId\":17,\"BuyerId\":51},{\"Id\":78,\"Name\":\"Imipramine Hydrochloride\",\"Price\":648.69,\"SellerId\":6,\"BuyerId\":46},{\"Id\":79,\"Name\":\"Aspirin\",\"Price\":925.45,\"SellerId\":1,\"BuyerId\":16},{\"Id\":80,\"Name\":\"Retin-A MICRO\",\"Price\":995.98,\"SellerId\":1,\"BuyerId\":9},{\"Id\":81,\"Name\":\"VITALUMIERE AQUA\",\"Price\":1293.09,\"SellerId\":8,\"BuyerId\":6},{\"Id\":82,\"Name\":\"Stila Hydrating Primer Oil-Free SPF 30\",\"Price\":179.28,\"SellerId\":20,\"BuyerId\":33},{\"Id\":83,\"Name\":\"Enchanted Moments Mistletoe Kisses Hand Sanitizer\",\"Price\":384.99,\"SellerId\":54,\"BuyerId\":22},{\"Id\":84,\"Name\":\"PCA SKIN ACNE\",\"Price\":1356.22,\"SellerId\":46,\"BuyerId\":3},{\"Id\":85,\"Name\":\"CALMING DIAPER RASH\",\"Price\":700.92,\"SellerId\":14,\"BuyerId\":23},{\"Id\":86,\"Name\":\"Labetalol hydrochloride\",\"Price\":436.38,\"SellerId\":38,\"BuyerId\":7},{\"Id\":87,\"Name\":\"Ketorolac Tromethamine\",\"Price\":608.18,\"SellerId\":22,\"BuyerId\":32},{\"Id\":88,\"Name\":\"Foaming Hand Sanitizer\",\"Price\":624.72,\"SellerId\":17,\"BuyerId\":28},{\"Id\":89,\"Name\":\"Aspergillus repens\",\"Price\":1231.42,\"SellerId\":49,\"BuyerId\":12},{\"Id\":90,\"Name\":\"ISOPROPYL ALCOHOL\",\"Price\":339.48,\"SellerId\":41,\"BuyerId\":18},{\"Id\":91,\"Name\":\"XtraCare Foam Antibacterial Hand Wash\",\"Price\":1251.97,\"SellerId\":12,\"BuyerId\":33},{\"Id\":92,\"Name\":\"smart sense nicotine\",\"Price\":1444.12,\"SellerId\":30,\"BuyerId\":11},{\"Id\":93,\"Name\":\"up and up temporary minor arthritis pain relief\",\"Price\":1085.78,\"SellerId\":13,\"BuyerId\":29},{\"Id\":94,\"Name\":\"RESCRIPTOR\",\"Price\":850.52,\"SellerId\":29,\"BuyerId\":3},{\"Id\":95,\"Name\":\"Buprenorphine hydrochloride\",\"Price\":391.05,\"SellerId\":35,\"BuyerId\":13},{\"Id\":96,\"Name\":\"PLANTAGO LANCEOLATA POLLEN\",\"Price\":561.68,\"SellerId\":47,\"BuyerId\":53},{\"Id\":97,\"Name\":\"Gehwol med Lipidro\",\"Price\":421.24,\"SellerId\":42,\"BuyerId\":46},{\"Id\":98,\"Name\":\"Ranitidine\",\"Price\":926.64,\"SellerId\":45,\"BuyerId\":33},{\"Id\":99,\"Name\":\"Carbon Dioxide Oxygen Mixture\",\"Price\":95.49,\"SellerId\":33,\"BuyerId\":6},{\"Id\":100,\"Name\":\"REYATAZ\",\"Price\":41.97,\"SellerId\":42,\"BuyerId\":3},{\"Id\":101,\"Name\":\"Allopurinol\",\"Price\":518.5,\"SellerId\":33,\"BuyerId\":25},{\"Id\":102,\"Name\":\"SEPHORA Acne-Fighting Mattifying Moisturizer\",\"Price\":1019.28,\"SellerId\":6,\"BuyerId\":31},{\"Id\":103,\"Name\":\"Smooth texture Orange flavor\",\"Price\":976.65,\"SellerId\":41,\"BuyerId\":12},{\"Id\":104,\"Name\":\"DAYWEAR PLUS MULTI PROTECTION TINTED MOISTURIZER\",\"Price\":555.12,\"SellerId\":55,\"BuyerId\":19},{\"Id\":105,\"Name\":\"Zonisamide\",\"Price\":1305.41,\"SellerId\":29,\"BuyerId\":3},{\"Id\":106,\"Name\":\"Peter Island Continous sunscreen kids\",\"Price\":471.3,\"SellerId\":27,\"BuyerId\":39},{\"Id\":107,\"Name\":\"GOONG SECRET CALMING BATH\",\"Price\":742.47,\"SellerId\":16,\"BuyerId\":30},{\"Id\":108,\"Name\":\"Clearskin\",\"Price\":968.59,\"SellerId\":11,\"BuyerId\":9},{\"Id\":109,\"Name\":\"No7 Protect and Perfect Foundation Sunscreen Broad Spectrum SPF 15 Cool Ivory\",\"Price\":616.19,\"SellerId\":5,\"BuyerId\":55},{\"Id\":110,\"Name\":\"Alcohol Free Antiseptic\",\"Price\":1486.07,\"SellerId\":39,\"BuyerId\":26},{\"Id\":111,\"Name\":\"smart sense nighttime cold and flu relief\",\"Price\":1101.77,\"SellerId\":9,\"BuyerId\":13},{\"Id\":112,\"Name\":\"Warfarin Sodium\",\"Price\":1379.79,\"SellerId\":39,\"BuyerId\":17},{\"Id\":113,\"Name\":\"Oxygen\",\"Price\":1242.92,\"SellerId\":13,\"BuyerId\":14},{\"Id\":114,\"Name\":\"Amlodipine Besylate\",\"Price\":122.57,\"SellerId\":15,\"BuyerId\":4},{\"Id\":115,\"Name\":\"CVS Therapeutic Menthol Pain Reliever\",\"Price\":1033.42,\"SellerId\":4,\"BuyerId\":3},{\"Id\":116,\"Name\":\"Warfarin Sodium\",\"Price\":770.05,\"SellerId\":24,\"BuyerId\":29},{\"Id\":117,\"Name\":\"Gilotrif\",\"Price\":1454.77,\"SellerId\":15,\"BuyerId\":47},{\"Id\":118,\"Name\":\"Shopko Lip Treatment\",\"Price\":861.42,\"SellerId\":29,\"BuyerId\":40},{\"Id\":119,\"Name\":\"Albuterol\",\"Price\":108.95,\"SellerId\":47,\"BuyerId\":45},{\"Id\":120,\"Name\":\"Eszopiclone\",\"Price\":405.03,\"SellerId\":3,\"BuyerId\":56},{\"Id\":121,\"Name\":\"EMEND\",\"Price\":1365.51,\"SellerId\":16,\"BuyerId\":36},{\"Id\":122,\"Name\":\"Etomidate\",\"Price\":393.94,\"SellerId\":6,\"BuyerId\":40},{\"Id\":123,\"Name\":\"TERSI\",\"Price\":554.91,\"SellerId\":12,\"BuyerId\":1},{\"Id\":124,\"Name\":\"Megestrol Acetate\",\"Price\":976.15,\"SellerId\":50,\"BuyerId\":28},{\"Id\":125,\"Name\":\"Glipizide and Metformin Hydrochloride\",\"Price\":953.6,\"SellerId\":50,\"BuyerId\":41},{\"Id\":126,\"Name\":\"Prednisone\",\"Price\":550.72,\"SellerId\":26,\"BuyerId\":44},{\"Id\":127,\"Name\":\"SNORING HP\",\"Price\":53.59,\"SellerId\":24,\"BuyerId\":30},{\"Id\":128,\"Name\":\"ROPINIROLE HYDROCHLORIDE\",\"Price\":266.44,\"SellerId\":33,\"BuyerId\":16},{\"Id\":129,\"Name\":\"kirkland signature minoxidil\",\"Price\":49.17,\"SellerId\":38,\"BuyerId\":39},{\"Id\":130,\"Name\":\"Agaricus Equisetum Special Order\",\"Price\":585.93,\"SellerId\":22,\"BuyerId\":27},{\"Id\":131,\"Name\":\"Lamotrigine Extended Release\",\"Price\":245.63,\"SellerId\":39,\"BuyerId\":38},{\"Id\":132,\"Name\":\"CLARINS Ever Matte SPF 15 - 105 Nude\",\"Price\":696.06,\"SellerId\":39,\"BuyerId\":56},{\"Id\":133,\"Name\":\"Childrens Allegra Allergy\",\"Price\":650.97,\"SellerId\":15,\"BuyerId\":36},{\"Id\":134,\"Name\":\"PredniSONE\",\"Price\":286.43,\"SellerId\":31,\"BuyerId\":32},{\"Id\":135,\"Name\":\"Spironolactone\",\"Price\":933.69,\"SellerId\":44,\"BuyerId\":3},{\"Id\":136,\"Name\":\"U-max Multi BB\",\"Price\":137.16,\"SellerId\":20,\"BuyerId\":35},{\"Id\":137,\"Name\":\"Phenylephrine HCl\",\"Price\":459.89,\"SellerId\":24,\"BuyerId\":53},{\"Id\":138,\"Name\":\"Finasteride\",\"Price\":1374.01,\"SellerId\":16,\"BuyerId\":55},{\"Id\":139,\"Name\":\"Clarins Paris Skin Illusion - 114 cappuccino\",\"Price\":811.42,\"SellerId\":28,\"BuyerId\":12},{\"Id\":140,\"Name\":\"Enalapril Maleate\",\"Price\":72.71,\"SellerId\":53,\"BuyerId\":24},{\"Id\":141,\"Name\":\"MEDICATED DANDRUFF\",\"Price\":1351.02,\"SellerId\":29,\"BuyerId\":42},{\"Id\":142,\"Name\":\"Pleo Lat\",\"Price\":720.08,\"SellerId\":7,\"BuyerId\":50},{\"Id\":143,\"Name\":\"Myristica Argentum Sinus Relief\",\"Price\":904.52,\"SellerId\":30,\"BuyerId\":47},{\"Id\":144,\"Name\":\"Glyburide\",\"Price\":95.1,\"SellerId\":16,\"BuyerId\":18},{\"Id\":145,\"Name\":\"Burn Jel\",\"Price\":209.57,\"SellerId\":34,\"BuyerId\":3},{\"Id\":146,\"Name\":\"CHAMOMILLA\",\"Price\":37.97,\"SellerId\":5,\"BuyerId\":48},{\"Id\":147,\"Name\":\"NEO-POLY-BAC HYDRO\",\"Price\":967.32,\"SellerId\":38,\"BuyerId\":48},{\"Id\":148,\"Name\":\"Isosorbide Mononitrate\",\"Price\":789.91,\"SellerId\":25,\"BuyerId\":10},{\"Id\":149,\"Name\":\"Head and Shoulders Conditioner\",\"Price\":1099.59,\"SellerId\":13,\"BuyerId\":36},{\"Id\":150,\"Name\":\"ERYTHROMYCIN Base Filmtab\",\"Price\":117.84,\"SellerId\":39,\"BuyerId\":3},{\"Id\":151,\"Name\":\"Flumazenil\",\"Price\":1151.37,\"SellerId\":22,\"BuyerId\":27},{\"Id\":152,\"Name\":\"Diastat\",\"Price\":614.14,\"SellerId\":32,\"BuyerId\":3},{\"Id\":153,\"Name\":\"Topical 60 Sec Sodium Fluoride\",\"Price\":1228.84,\"SellerId\":17,\"BuyerId\":4},{\"Id\":154,\"Name\":\"TRAMADOL HYDROCHLORIDE\",\"Price\":516.48,\"SellerId\":28,\"BuyerId\":3},{\"Id\":155,\"Name\":\"Fexofenadine HCl and Pseudoephedrine HCI\",\"Price\":73.07,\"SellerId\":42,\"BuyerId\":1},{\"Id\":156,\"Name\":\"equaline\",\"Price\":520.45,\"SellerId\":26,\"BuyerId\":11},{\"Id\":157,\"Name\":\"Leg Cramp Relief\",\"Price\":1345.69,\"SellerId\":46,\"BuyerId\":15},{\"Id\":158,\"Name\":\"CD CAPTURE TOTALE Triple Correcting Serum Foundation Wrinkles-Dark Spots-Radiance with sunscreen Broad Spectrum SPF 25 010\",\"Price\":77.23,\"SellerId\":40,\"BuyerId\":4},{\"Id\":159,\"Name\":\"PRIMAXIN\",\"Price\":686.66,\"SellerId\":9,\"BuyerId\":22},{\"Id\":160,\"Name\":\"H-Rosacea Formula\",\"Price\":99.74,\"SellerId\":54,\"BuyerId\":49},{\"Id\":161,\"Name\":\"Benazepril Hydrochloride and Hydrochlorothiazide\",\"Price\":1187.27,\"SellerId\":5,\"BuyerId\":22},{\"Id\":162,\"Name\":\"Leflunomide\",\"Price\":312.85,\"SellerId\":53,\"BuyerId\":4},{\"Id\":163,\"Name\":\"BareMinerals Golden Tan matte\",\"Price\":110.93,\"SellerId\":15,\"BuyerId\":46},{\"Id\":164,\"Name\":\"CEDAX\",\"Price\":342.86,\"SellerId\":27,\"BuyerId\":3},{\"Id\":165,\"Name\":\"Topex\",\"Price\":1258.49,\"SellerId\":1,\"BuyerId\":10},{\"Id\":166,\"Name\":\"DIVALPROEX SODIUM\",\"Price\":1287.03,\"SellerId\":7,\"BuyerId\":40},{\"Id\":167,\"Name\":\"Acetic Acid\",\"Price\":1060.43,\"SellerId\":26,\"BuyerId\":35},{\"Id\":168,\"Name\":\"MEDI-FIRST Non-Aspirin\",\"Price\":1301.28,\"SellerId\":53,\"BuyerId\":52},{\"Id\":169,\"Name\":\"Lorazepam\",\"Price\":1134.96,\"SellerId\":47,\"BuyerId\":10},{\"Id\":170,\"Name\":\"Alternaria alternata\",\"Price\":61.24,\"SellerId\":52,\"BuyerId\":35},{\"Id\":171,\"Name\":\"Budpak Hemorrhoid Anesthetic\",\"Price\":1499.29,\"SellerId\":48,\"BuyerId\":56},{\"Id\":172,\"Name\":\"Ringers\",\"Price\":1054.37,\"SellerId\":22,\"BuyerId\":30},{\"Id\":173,\"Name\":\"LBEL Couleur Luxe Rouge Amplifier XP amplifying SPF 15\",\"Price\":1069.43,\"SellerId\":7,\"BuyerId\":6},{\"Id\":174,\"Name\":\"Metformin Hydrochloride\",\"Price\":953.99,\"SellerId\":35,\"BuyerId\":12},{\"Id\":175,\"Name\":\"Cold and Cough\",\"Price\":218.14,\"SellerId\":4,\"BuyerId\":43},{\"Id\":176,\"Name\":\"PANTOPRAZOLE SODIUM\",\"Price\":293.89,\"SellerId\":25,\"BuyerId\":40},{\"Id\":177,\"Name\":\"LANEIGE MYSTIC VEIL FOUNDATION\",\"Price\":20.86,\"SellerId\":43,\"BuyerId\":28},{\"Id\":178,\"Name\":\"EPZICOM\",\"Price\":895.65,\"SellerId\":34,\"BuyerId\":35},{\"Id\":179,\"Name\":\"Almond\",\"Price\":367.32,\"SellerId\":33,\"BuyerId\":9},{\"Id\":180,\"Name\":\"Etoposide\",\"Price\":1483.96,\"SellerId\":41,\"BuyerId\":27},{\"Id\":181,\"Name\":\"ESIKA 3-IN-1 PRO MAKE UP FOUNDATION SPF 20 BASE DE MAQUILLAJE PARA ROSTRO 3-EN-1 PRO FPS 20\",\"Price\":1097.6,\"SellerId\":46,\"BuyerId\":10},{\"Id\":182,\"Name\":\"Levothyroxine Sodium\",\"Price\":885.86,\"SellerId\":48,\"BuyerId\":15},{\"Id\":183,\"Name\":\"Dawn Ultra Antibacterial Hand\",\"Price\":969.86,\"SellerId\":56,\"BuyerId\":51},{\"Id\":184,\"Name\":\"cough and sore throat\",\"Price\":1482.68,\"SellerId\":9,\"BuyerId\":11},{\"Id\":185,\"Name\":\"Moore Medical Sinus Pain and Pressure Relief\",\"Price\":855.39,\"SellerId\":43,\"BuyerId\":3},{\"Id\":186,\"Name\":\"Glipizide\",\"Price\":621.78,\"SellerId\":4,\"BuyerId\":1},{\"Id\":187,\"Name\":\"Yellow Jacket hymenoptera venom Venomil Diagnostic\",\"Price\":23.58,\"SellerId\":22,\"BuyerId\":3},{\"Id\":188,\"Name\":\"Fosphenytoin Sodium\",\"Price\":1334.06,\"SellerId\":37,\"BuyerId\":25},{\"Id\":189,\"Name\":\"LV-FX\",\"Price\":820.87,\"SellerId\":48,\"BuyerId\":40},{\"Id\":190,\"Name\":\"Wintergreen Isopropyl Alcohol\",\"Price\":1397.57,\"SellerId\":8,\"BuyerId\":26},{\"Id\":191,\"Name\":\"ORCHID SECRET PACT\",\"Price\":59.53,\"SellerId\":42,\"BuyerId\":19},{\"Id\":192,\"Name\":\"Metaxalone\",\"Price\":79.94,\"SellerId\":6,\"BuyerId\":19},{\"Id\":193,\"Name\":\"Allergena\",\"Price\":109.32,\"SellerId\":16,\"BuyerId\":38},{\"Id\":194,\"Name\":\"XANTHIUM STRUMARIUM VAR CANADENSE POLLEN\",\"Price\":1091.38,\"SellerId\":54,\"BuyerId\":35},{\"Id\":195,\"Name\":\"Ampicillin\",\"Price\":674.63,\"SellerId\":49,\"BuyerId\":3},{\"Id\":196,\"Name\":\"Buprenorphine hydrochloride and naloxone hydrochloride dihydrate\",\"Price\":293.33,\"SellerId\":45,\"BuyerId\":18},{\"Id\":197,\"Name\":\"Acnezzol Base\",\"Price\":710.6,\"SellerId\":51,\"BuyerId\":46},{\"Id\":198,\"Name\":\"AMARANTHUS PALMERI POLLEN\",\"Price\":623.16,\"SellerId\":28,\"BuyerId\":32},{\"Id\":199,\"Name\":\"ENALAPRIL MALEATE\",\"Price\":210.42,\"SellerId\":51,\"BuyerId\":14},{\"Id\":200,\"Name\":\"Acetaminophen, Chlorpheniramine Maleate, Dextromethorphan Hydrobromide, Phenylephrine Hydrochloride\",\"Price\":1028.27,\"SellerId\":50,\"BuyerId\":3}],\"CategoryProduct\":[{\"CategoryId\":4,\"ProductId\":1},{\"CategoryId\":11,\"ProductId\":2},{\"CategoryId\":6,\"ProductId\":3},{\"CategoryId\":5,\"ProductId\":4},{\"CategoryId\":4,\"ProductId\":5},{\"CategoryId\":11,\"ProductId\":6},{\"CategoryId\":5,\"ProductId\":7},{\"CategoryId\":8,\"ProductId\":8},{\"CategoryId\":8,\"ProductId\":9},{\"CategoryId\":5,\"ProductId\":10},{\"CategoryId\":5,\"ProductId\":11},{\"CategoryId\":5,\"ProductId\":12},{\"CategoryId\":5,\"ProductId\":13},{\"CategoryId\":9,\"ProductId\":14},{\"CategoryId\":6,\"ProductId\":15},{\"CategoryId\":4,\"ProductId\":16},{\"CategoryId\":6,\"ProductId\":17},{\"CategoryId\":5,\"ProductId\":18},{\"CategoryId\":7,\"ProductId\":19},{\"CategoryId\":1,\"ProductId\":20},{\"CategoryId\":4,\"ProductId\":21},{\"CategoryId\":2,\"ProductId\":22},{\"CategoryId\":10,\"ProductId\":23},{\"CategoryId\":2,\"ProductId\":24},{\"CategoryId\":10,\"ProductId\":25},{\"CategoryId\":5,\"ProductId\":26},{\"CategoryId\":6,\"ProductId\":27},{\"CategoryId\":2,\"ProductId\":28},{\"CategoryId\":8,\"ProductId\":29},{\"CategoryId\":7,\"ProductId\":30},{\"CategoryId\":2,\"ProductId\":31},{\"CategoryId\":4,\"ProductId\":32},{\"CategoryId\":4,\"ProductId\":33},{\"CategoryId\":1,\"ProductId\":34},{\"CategoryId\":3,\"ProductId\":35},{\"CategoryId\":1,\"ProductId\":36},{\"CategoryId\":2,\"ProductId\":37},{\"CategoryId\":5,\"ProductId\":38},{\"CategoryId\":11,\"ProductId\":39},{\"CategoryId\":7,\"ProductId\":40},{\"CategoryId\":9,\"ProductId\":41},{\"CategoryId\":2,\"ProductId\":42},{\"CategoryId\":7,\"ProductId\":43},{\"CategoryId\":4,\"ProductId\":44},{\"CategoryId\":10,\"ProductId\":45},{\"CategoryId\":7,\"ProductId\":46},{\"CategoryId\":2,\"ProductId\":47},{\"CategoryId\":2,\"ProductId\":48},{\"CategoryId\":3,\"ProductId\":49},{\"CategoryId\":4,\"ProductId\":50},{\"CategoryId\":10,\"ProductId\":51},{\"CategoryId\":9,\"ProductId\":52},{\"CategoryId\":8,\"ProductId\":53},{\"CategoryId\":11,\"ProductId\":54},{\"CategoryId\":11,\"ProductId\":55},{\"CategoryId\":11,\"ProductId\":56},{\"CategoryId\":10,\"ProductId\":57},{\"CategoryId\":2,\"ProductId\":58},{\"CategoryId\":4,\"ProductId\":59},{\"CategoryId\":8,\"ProductId\":60},{\"CategoryId\":2,\"ProductId\":61},{\"CategoryId\":1,\"ProductId\":62},{\"CategoryId\":9,\"ProductId\":63},{\"CategoryId\":10,\"ProductId\":64},{\"CategoryId\":1,\"ProductId\":65},{\"CategoryId\":9,\"ProductId\":66},{\"CategoryId\":10,\"ProductId\":67},{\"CategoryId\":1,\"ProductId\":68},{\"CategoryId\":4,\"ProductId\":69},{\"CategoryId\":3,\"ProductId\":70},{\"CategoryId\":11,\"ProductId\":71},{\"CategoryId\":10,\"ProductId\":72},{\"CategoryId\":3,\"ProductId\":73},{\"CategoryId\":8,\"ProductId\":74},{\"CategoryId\":1,\"ProductId\":75},{\"CategoryId\":10,\"ProductId\":76},{\"CategoryId\":4,\"ProductId\":77},{\"CategoryId\":4,\"ProductId\":78},{\"CategoryId\":11,\"ProductId\":79},{\"CategoryId\":11,\"ProductId\":80},{\"CategoryId\":11,\"ProductId\":81},{\"CategoryId\":2,\"ProductId\":82},{\"CategoryId\":8,\"ProductId\":83},{\"CategoryId\":1,\"ProductId\":84},{\"CategoryId\":5,\"ProductId\":85},{\"CategoryId\":2,\"ProductId\":86},{\"CategoryId\":4,\"ProductId\":87},{\"CategoryId\":1,\"ProductId\":88},{\"CategoryId\":10,\"ProductId\":89},{\"CategoryId\":11,\"ProductId\":90},{\"CategoryId\":10,\"ProductId\":91},{\"CategoryId\":8,\"ProductId\":92},{\"CategoryId\":9,\"ProductId\":93},{\"CategoryId\":1,\"ProductId\":94},{\"CategoryId\":3,\"ProductId\":95},{\"CategoryId\":5,\"ProductId\":96},{\"CategoryId\":2,\"ProductId\":97},{\"CategoryId\":5,\"ProductId\":98},{\"CategoryId\":1,\"ProductId\":99},{\"CategoryId\":9,\"ProductId\":100},{\"CategoryId\":5,\"ProductId\":101},{\"CategoryId\":9,\"ProductId\":102},{\"CategoryId\":7,\"ProductId\":103},{\"CategoryId\":8,\"ProductId\":104},{\"CategoryId\":1,\"ProductId\":105},{\"CategoryId\":8,\"ProductId\":106},{\"CategoryId\":4,\"ProductId\":107},{\"CategoryId\":8,\"ProductId\":108},{\"CategoryId\":8,\"ProductId\":109},{\"CategoryId\":9,\"ProductId\":110},{\"CategoryId\":5,\"ProductId\":111},{\"CategoryId\":4,\"ProductId\":112},{\"CategoryId\":10,\"ProductId\":113},{\"CategoryId\":1,\"ProductId\":114},{\"CategoryId\":5,\"ProductId\":115},{\"CategoryId\":9,\"ProductId\":116},{\"CategoryId\":2,\"ProductId\":117},{\"CategoryId\":5,\"ProductId\":118},{\"CategoryId\":2,\"ProductId\":119},{\"CategoryId\":7,\"ProductId\":120},{\"CategoryId\":3,\"ProductId\":121},{\"CategoryId\":7,\"ProductId\":122},{\"CategoryId\":11,\"ProductId\":123},{\"CategoryId\":1,\"ProductId\":124},{\"CategoryId\":1,\"ProductId\":125},{\"CategoryId\":11,\"ProductId\":126},{\"CategoryId\":9,\"ProductId\":127},{\"CategoryId\":10,\"ProductId\":128},{\"CategoryId\":3,\"ProductId\":129},{\"CategoryId\":4,\"ProductId\":130},{\"CategoryId\":6,\"ProductId\":131},{\"CategoryId\":7,\"ProductId\":132},{\"CategoryId\":11,\"ProductId\":133},{\"CategoryId\":10,\"ProductId\":134},{\"CategoryId\":4,\"ProductId\":135},{\"CategoryId\":11,\"ProductId\":136},{\"CategoryId\":3,\"ProductId\":137},{\"CategoryId\":8,\"ProductId\":138},{\"CategoryId\":9,\"ProductId\":139},{\"CategoryId\":6,\"ProductId\":140},{\"CategoryId\":7,\"ProductId\":141},{\"CategoryId\":4,\"ProductId\":142},{\"CategoryId\":1,\"ProductId\":143},{\"CategoryId\":9,\"ProductId\":144},{\"CategoryId\":4,\"ProductId\":145},{\"CategoryId\":4,\"ProductId\":146},{\"CategoryId\":6,\"ProductId\":147},{\"CategoryId\":6,\"ProductId\":148},{\"CategoryId\":4,\"ProductId\":149},{\"CategoryId\":2,\"ProductId\":150},{\"CategoryId\":1,\"ProductId\":151},{\"CategoryId\":5,\"ProductId\":152},{\"CategoryId\":1,\"ProductId\":153},{\"CategoryId\":2,\"ProductId\":154},{\"CategoryId\":4,\"ProductId\":155},{\"CategoryId\":7,\"ProductId\":156},{\"CategoryId\":7,\"ProductId\":157},{\"CategoryId\":3,\"ProductId\":158},{\"CategoryId\":5,\"ProductId\":159},{\"CategoryId\":9,\"ProductId\":160},{\"CategoryId\":8,\"ProductId\":161},{\"CategoryId\":2,\"ProductId\":162},{\"CategoryId\":2,\"ProductId\":163},{\"CategoryId\":1,\"ProductId\":164},{\"CategoryId\":1,\"ProductId\":165},{\"CategoryId\":6,\"ProductId\":166},{\"CategoryId\":1,\"ProductId\":167},{\"CategoryId\":9,\"ProductId\":168},{\"CategoryId\":6,\"ProductId\":169},{\"CategoryId\":3,\"ProductId\":170},{\"CategoryId\":3,\"ProductId\":171},{\"CategoryId\":2,\"ProductId\":172},{\"CategoryId\":5,\"ProductId\":173},{\"CategoryId\":5,\"ProductId\":174},{\"CategoryId\":9,\"ProductId\":175},{\"CategoryId\":7,\"ProductId\":176},{\"CategoryId\":3,\"ProductId\":177},{\"CategoryId\":10,\"ProductId\":178},{\"CategoryId\":6,\"ProductId\":179},{\"CategoryId\":5,\"ProductId\":180},{\"CategoryId\":2,\"ProductId\":181},{\"CategoryId\":11,\"ProductId\":182},{\"CategoryId\":3,\"ProductId\":183},{\"CategoryId\":10,\"ProductId\":184},{\"CategoryId\":11,\"ProductId\":185},{\"CategoryId\":10,\"ProductId\":186},{\"CategoryId\":3,\"ProductId\":187},{\"CategoryId\":7,\"ProductId\":188},{\"CategoryId\":4,\"ProductId\":189},{\"CategoryId\":5,\"ProductId\":190},{\"CategoryId\":1,\"ProductId\":191},{\"CategoryId\":9,\"ProductId\":192},{\"CategoryId\":11,\"ProductId\":193},{\"CategoryId\":8,\"ProductId\":194},{\"CategoryId\":6,\"ProductId\":195},{\"CategoryId\":2,\"ProductId\":196},{\"CategoryId\":11,\"ProductId\":197},{\"CategoryId\":8,\"ProductId\":198},{\"CategoryId\":6,\"ProductId\":199},{\"CategoryId\":6,\"ProductId\":200}]}";

        var datasets = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<JObject>>>(datasetsJson);

        foreach (var dataset in datasets)
        {
            var entityType = GetType(dataset.Key);
            var entities = dataset.Value
                .Select(j => j.ToObject(entityType))
                .ToArray();

            context.AddRange(entities);
        }

        context.SaveChanges();
    }

    private static Type GetType(string modelName)
    {
        var modelType = CurrentAssembly
            .GetTypes()
            .FirstOrDefault(t => t.Name == modelName);

        Assert.IsNotNull(modelType, $"{modelName} model not found!");

        return modelType;
    }

    private static IServiceProvider ConfigureServices<TContext>(string databaseName)
        where TContext : DbContext
    {
        var services = ConfigureDbContext<TContext>(databaseName);

        var context = services.GetService<TContext>();

        try
        {
            context.Model.GetEntityTypes();
        }
        catch (InvalidOperationException ex) when (ex.Source == "Microsoft.EntityFrameworkCore.Proxies")
        {
            services = ConfigureDbContext<TContext>(databaseName, useLazyLoading: true);
        }

        return services;
    }

    private static IServiceProvider ConfigureDbContext<TContext>(string databaseName, bool useLazyLoading = false)
        where TContext : DbContext
    {
        var services = new ServiceCollection();

        services
            .AddDbContext<TContext>(
                options => options
                    .UseInMemoryDatabase(databaseName)
                    .UseLazyLoadingProxies(useLazyLoading)
            );

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}