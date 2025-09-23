using System;
using System.Collections.Generic;

public static class LocalizedText
{
    public static Dictionary<string, (string en, string tl)> TagNames = new()
    {
        { "skullDescriptionCon", ("Skull", "Bungo") },
        { "ribsDescriptionCon", ("Ribs", "Tadyang") },
        { "clavicleDescriptionCon", ("Clavicle", "Klawikula") },
        { "sternumDescriptionCon", ("Sternum", "Esternum") },
        { "scapulaDescriptionCon", ("Scapula", "Eskapula") },
        { "humerusDescriptionCon", ("Humerus", "Buto ng braso") },
        { "spineDescriptionCon", ("Spine", "Bertebra / Gulugod") },
        { "radiusDescriptionCon", ("Radius", "Buto sa labas ng bisig") },
        { "ulnaDescriptionCon", ("Ulna", "Buto sa loob ng bisig") },
        { "carpalsDescriptionCon", ("Carpals", "Maliit na buto sa pulso") },
        { "metacarpalsDescriptionCon", ("Metacarpals", "Gitnang buto ng kamay") },
        { "phalangeFingersDescriptionCon", ("Phalange", "Buto ng mga daliri") },
        { "femurDescriptionCon", ("Femur", "Buto ng hita") },
        { "patellaDescriptionCon", ("Patella", "Tuhod / Batella") },
        { "tibiaDescriptionCon", ("Tibia", "Harapang buto ng binti") },
        { "fibulaDescriptionCon", ("Fibula", "Likurang buto ng binti") },
        { "tarsalDescriptionCon", ("Tarsal", "Maliit na buto sa sakong") },
        { "metatarsalDescriptionCon", ("Metatarsal", "Gitnang buto ng paa") },
        { "phalangeToesDescriptionCon", ("Phalange", "Buto ng mga daliri") },
        { "coccyxDescriptionCon", ("Coccyx", "Buntot ng buto")},
        { "pelvicGirdleDescriptionCon", ("Pelvic Girdle", "Balangkas ng balakang")},

        // Integumentary
      { "hairShaftDescriptionCon", ("Hair Shaft", "Hibla ng buhok") },
        { "sweatGlandDescriptionCon", ("Sweat Gland", "Ugat ng pawis") },
        { "hairRootDescriptionCon", ("Hair Root", "Ugat ng buhok") },
        { "poreOfGlandDescriptionCon", ("Pore of Gland", "Poro ng pawis") },
        { "epidermisDescriptionCon", ("Epidermis", "Panlabas na balat") },
        { "dermisDescriptionCon", ("Dermis", "Gitnang balat") },
        { "hypodermisDescriptionCon", ("Hypodermis", "Ilalim na balat") },

       // Digestive System
        { "mouthDescriptionCon", ("Mouth", "Bibig") },
        { "esophagusDescriptionCon", ("Esophagus", "Lagusang ng pagkain") },
        { "stomachDescriptionCon", ("Stomach", "Tiyan / Sikmura") },
        { "largeIntestineDescriptionCon", ("Large Intestine", "Malaking bituka") },
        { "smallIntestineDescriptionCon", ("Small Intestine", "Maliit na bituka") },
        { "rectumDescriptionCon", ("Rectum", "Rektum") },
  
       // Respiratory System 
        { "nasalCavityDescriptionCon", ("Nasal Cavity", "Butas ng ilong") },
        { "lungsDescriptionCon", ("Lungs", "Baga") },
        { "pharynxDescriptionCon", ("Pharynx", "Parinks / Lalamunan") },
        { "larynxDescriptionCon", ("Larynx", "Larinx / Kahon ng Tinig") },
        { "tracheaDescriptionCon", ("Trachea", "Trakea / Lagusan ng Hangin") },
        { "bronchiDescriptionCon", ("Bronchi", "Bronkiyo / Sanga ng Trakea") },
    //----------------
        // Nervous
        { "cerebrumDescriptionCon", ("Cerebrum", "Serebrum") },
        { "hypothalamusDescriptionCon", ("Hypothalamus", "Hypothalamus") },
        { "medullaOblongataDescriptionCon", ("Medulla Oblongata", "Medulla Oblongata") },
        { "cerebellumDescriptionCon", ("Cerebellum", "Serebelum") },
        { "brainStemDescriptionCon", ("Brain Stem", "Brain Stem") },
        
        // Circulatory System Tag Names
      { "bloodVesselsDescriptionCon", ("Blood Vessels", "Ugat ng dugo") },
        { "veinsDescriptionCon", ("Veins", "Bena") },
        { "capillariesDescriptionCon", ("Capillaries", "Kapilaryo") },
        { "arteriesDescriptionCon", ("Arteries", "Arterya") },
        { "heartDescriptionCon", ("Heart", "Puso") },

        // Circulatory - Heart 
        { "rightVentricleDescriptionCon", ("Right Ventricle", "Kanang Ventricle") },
        { "rightAtriumDescriptionCon", ("Right Atrium", "Kanang Atrium") },
        { "leftAtriumDescriptionCon", ("Left Atrium", "Kaliwang Atrium") },
        { "leftVentricleDescriptionCon", ("Left Ventricle", "Kaliwang Ventricle") },
        { "superiorVenaCavaDescriptionCon", ("Superior Vena Cava", "Pang-itaas ng ugat ng dugo") },
        { "inferiorVenaCavaDescriptionCon", ("Inferior Vena Cava", "Pang-ibabang ugat ng dugo") },
        { "pulmonaryVeinDescriptionCon", ("Pulmonary Vein", "Ugat mula sa Baga") },
        { "pulmonaryArteryDescriptionCon", ("Pulmonary Artery", "Ugat papunta sa Baga") },
        { "tricuspidValveDescriptionCon", ("Tricuspid Valve", "Tatlong-labing Balbula") },
        { "pulmonaryValveDescriptionCon", ("Pulmonary Valve", "Balbula ng baga") },
        { "mitralValveDescriptionCon", ("Mitral Valve", "Balbula ng mitral") },
        { "aorticValveDescriptionCon", ("Aortic Valve", "Balbula ng aorta") },

        // Excretory
        {"kidneysDescriptionCon",("Kidneys", "Mga Bato")},
        {"uretersDescriptionCon",("Ureters", "Ureters")},
        {"urinaryBladderDescriptionCon",("Urinary Bladder", "Pantog")},
        {"urethraDescriptionCon",("Urethra", "Urethra")},
        
    };
    public static Dictionary<string, (string en, string tl)> TagDescriptions = new()
    {
        // Skeletal
        { "skullDescriptionCon", ("The skull is a hard, strong bone that protects your brain like a helmet. It also gives shape to your face, holding and supporting important features like your eyes, nose, and mouth.", "Ang bungo ay isang matigas at matibay na buto na nagpoprotekta sa iyong utak tulad ng helmet. Ito rin ang nagbibigay-hugis sa iyong mukha at sumusuporta sa mga mata, ilong, at bibig.") },
        { "ribsDescriptionCon", ("Twelve pairs of curved bones wrap your chest. They protect your lungs and heart, and help your chest expand when you breathe.", "Labindalawang pares ng hubog na buto ang bumabalot sa dibdib. Pinoprotektahan nito ang baga at puso, at tumutulong sa paglawak ng dibdib habang humihinga.") },
        { "clavicleDescriptionCon", ("A curved bone at the top of your chest that connects to your shoulder. It keeps your arm in place and helps it move freely.", "Isang hubog na buto sa itaas ng dibdib na konektado sa balikat. Pinapanatili nito sa tamang posisyon ang braso at tinutulungan itong gumalaw nang malaya.") },
        { "sternumDescriptionCon", ("A flat bone in the middle of your chest. It connects your ribs and guards your heart and lungs like a shield.", "Isang patag na buto sa gitna ng dibdib. Kinakabitan ito ng mga tadyang at nagsisilbing panangga sa puso at baga, parang isang kalasag.") },
        { "scapulaDescriptionCon", ("A flat, triangular bone at the back. It works with the collarbone to let your arm move up, down, and around.", "Isang patag at tatsulok na buto sa likod. Kasama ng kulang-palad, tumutulong ito sa paggalaw ng braso pataas, pababa, at paikot.") },
        { "humerusDescriptionCon", ("The long bone in your upper arm (shoulder to elbow). It lets you lift and swing with strength.", "Ang humerus ay ang mahabang buto sa itaas na bahagi ng braso, mula balikat hanggang siko. Tinutulungan ka nitong buhatin at igalaw ang iyong braso nang may lakas.") },
        { "spineDescriptionCon", ("A chain of small bones from your neck to your lower back. It supports your body and lets you bend and twist", "Isang hanay ng maliliit na buto mula leeg hanggang ibabang likod. Sinusuportahan nito ang katawan at tumutulong sa pagyuko at pag-ikot.") },
        { "radiusDescriptionCon", ("Located at the thumb side of your arm and it helps you twist your wrist.", "Matatagpuan sa bahagi ng hinlalaki ng iyong braso. Tinutulungan ka nitong igalaw o iikot ang iyong pulso.") },
        { "ulnaDescriptionCon", ("Located at the little-finger side of your arm and it helps your elbow bend.", "Matatagpuan sa bahagi ng maliit na daliri sa iyong braso. Tinutulungan nito ang siko na yumuko.") },
        { "carpalsDescriptionCon", ("Eight small bones in your wrist that let your hand bend and twist.", "Walong maliliit na buto sa iyong pulso na nagbibigay-daan sa pagyuko at pag-ikot ng kamay.") },
        { "metacarpalsDescriptionCon", ("Five bones in your palm that help your fingers move.", "Limang buto sa palad na tumutulong sa paggalaw ng mga daliri.") },
        { "phalangeFingersDescriptionCon", ("Fingers each have three bones, except thumbs which have two. They help you grab, write, and play.", "Ang bawat daliri ay may tatlong buto, maliban sa hinlalaki na may dalawa. Tinutulungan ka nitong humawak, magsulat, at maglaro.") },
        { "femurDescriptionCon", ("The longest and strongest bone in your body—from hip to knee. It supports walking, running, and jumping.", "Ang pinakamahaba at pinakamalakas na buto sa katawan—mula balakang hanggang tuhod. Sinusuportahan nito ang paglalakad, pagtakbo, at pagtalon.") },
        { "patellaDescriptionCon", ("A triangle-shaped bone at the knee. It protects your knee when you bend or kneel.", "Isang tatsulok na buto sa tuhod. Pinoprotektahan nito ang tuhod kapag ikaw ay yumuyuko o lumuluhod.") },
        { "tibiaDescriptionCon", ("Supports most of your weight.", "Ito ang pangunahing buto sa ibabang bahagi ng binti na sumusuporta sa karamihan ng bigat ng iyong katawan.") },
        { "fibulaDescriptionCon", ("A thinner bone beside the tibia—it helps keep you balanced.", "Isang mas manipis na buto sa tabi ng tibia. Tinutulungan ka nitong mapanatili ang balanse.") },
        { "tarsalDescriptionCon", ("Seven bones in your ankle that allow your foot to move up and down.", "Pitong buto sa iyong sakong na nagpapahintulot sa paggalaw ng paa paitaas at paibaba.") },
        { "metatarsalDescriptionCon", ("Five bones in the middle of your foot that help with walking and balance.", "Limang buto sa gitna ng iyong paa na tumutulong sa paglalakad at pagpapanatili ng balanse.") },
        { "phalangeToesDescriptionCon", ("Toes each have three bones, except big toes with two. They help you balance and push off the ground when walking.", "Ang bawat daliri sa paa ay may tatlong buto, maliban sa hinlalaki ng paa na may dalawa. Tinutulungan ka nitong magpanatili ng balanse at tumulak mula sa lupa habang naglalakad.") },
        { "coccyxDescriptionCon", ("A small bone at the bottom of your spine. It helps you balance when you sit.", "Isang maliit na buto sa dulo ng iyong gulugod. Tinutulungan ka nitong manatiling balanse kapag nakaupo.") },
        { "pelvicGirdleDescriptionCon", ("The pelvic girdle is a ring of bones located at the base of the spine that connects the trunk of the body to the legs. It consists of the two hip bones (ilium, ischium, and pubis), sacrum, and coccyx. The pelvic girdle supports the weight of the upper body and protects internal organs like the bladder and reproductive organs. It also provides attachment points for muscles used in movement and posture.", "Ang pelvic girdle ay isang hugis-sing-sing na buto na matatagpuan sa ibaba ng gulugod at nag-uugnay sa katawan at mga binti. Binubuo ito ng dalawang hip bone (ilium, ischium, at pubis), sacrum, at coccyx. Sinusuportahan nito ang bigat ng katawan at nagbibigay-proteksyon sa mga internal na organ tulad ng pantog at reproductive organs. Nagsisilbi rin itong pagkakabitan ng mga kalamnan para sa paggalaw at tamang postura.") },

        // ----------------------------
        // Integumentary
        { "hairShaftDescriptionCon", ("This is the part of the hair you can see above your skin. It helps protect your head from the sun and keeps you warm.", "Ito ang bahagi ng buhok na makikita sa ibabaw ng balat. Pinoprotektahan nito ang iyong ulo mula sa araw at tumutulong na mapanatiling mainit ang katawan.") },
        { "sweatGlandDescriptionCon", ("These are tiny coils under your skin that make sweat to help cool your body when you’re hot.", "Ito ay maliliit na likaw sa ilalim ng balat na gumagawa ng pawis upang palamigin ang katawan kapag mainit.") },
        { "hairRootDescriptionCon", ("This is the part of the hair under the skin. It’s where the hair starts growing, and it's held in a tiny sac called a hair follicle.", "Ito ang bahagi ng buhok sa ilalim ng balat. Dito nagsisimulang tumubo ang buhok at nakapaloob ito sa maliit na sisidlan na tinatawag na hair follicle.") },
        { "poreOfGlandDescriptionCon", ("This is the small opening on your skin where sweat comes out. It helps your body release heat.", "Ito ang maliit na butas sa balat kung saan lumalabas ang pawis. Tinutulungan nitong ilabas ang init ng katawan.") },
        { "epidermisDescriptionCon", ("The thin outer layer of your skin. It protects your body and keeps germs and dirt out.", "Ang manipis na panlabas na bahagi ng balat. Pinoprotektahan nito ang katawan laban sa dumi at mikrobyo.") },
        { "dermisDescriptionCon", ("The middle layer of the skin under the epidermis. It contains blood vessels, sweat glands, hair roots, and nerves.", "Ang gitnang bahagi ng balat na nasa ilalim ng epidermis. Dito matatagpuan ang mga ugat ng dugo, glandula ng pawis, ugat ng buhok, at mga nerbiyos.") },
        { "hypodermisDescriptionCon", ("This is the bottom layer of skin. It has fat that helps keep you warm and protect your organs.", "Ito ang pinakailalim na bahagi ng balat. Mayroon itong taba na tumutulong panatilihing mainit ang katawan at protektahan ang mga laman-loob.") },

        // Digestive
        { "mouthDescriptionCon", ("Digestion starts here. The teeth cut and grind food while saliva moistens and begins breaking down starch.", "Dito nagsisimula ang pagtunaw ng pagkain. Pinuputol at pinupulbos ng mga ngipin ang pagkain habang ang laway ay nagpapalambot at tumutunaw sa almirol.") },
        { "esophagusDescriptionCon", ("A muscular food tube that connects the mouth to the stomach. It pushes food down using wave-like movements.", "Isang masel na tubo na nag-uugnay sa bibig at tiyan. Pinatutulak nito ang pagkain pababa sa pamamagitan ng alon-alon na paggalaw.") },
        { "stomachDescriptionCon", ("A muscular pouch where food is mixed with gastric juice and broken down into smaller pieces.", "Isang masel na sisidlan kung saan hinahalo ang pagkain sa gastric juice at pinaghihiwa-hiwalay ito sa mas maliliit na piraso.") },
        { "largeIntestineDescriptionCon", ("This wide tube absorbs extra water and forms the waste into solid poop.", "Ang malawak na tubong ito ay sumisipsip ng sobrang tubig at hinuhubog ang dumi upang maging buo.") },
        { "smallIntestineDescriptionCon", ("A long, coiled tube where most digestion happens. Nutrients from the food are absorbed here.", "Isang mahabang paikot-ikot na tubo kung saan nagaganap ang karamihan ng pagtunaw. Dito hinihigop ang mga sustansya mula sa pagkain.") },
        { "rectumDescriptionCon", ("The final part of the large intestine. It stores poop until it’s ready to leave the body.", "Ang huling bahagi ng malaking bituka. Dito pansamantalang iniimbak ang dumi bago ito ilabas sa katawan.") },

        // Respiratory
        { "pharynxDescriptionCon", ("Pharynx is also called the throat. The common passageway for both food, water, and air.", "Ang pharynx ay kilala rin bilang lalamunan. Ito ang karaniwang daanan ng pagkain, tubig, at hangin.") },
        { "nasalCavityDescriptionCon", ("The nostrils are the opening into the nasal passages that are lined with hairs. The nasal cavity is lined by glands that produce sticky mucus. Dust, pollen, and other materials are trapped by mucus. This trapping of air impurities helps in filtering the air you breathe.", "Ang butas ng ilong ay ang pasukan sa mga lagusan ng ilong na may balahibo. May mga glandula sa loob ng ilong na gumagawa ng malagkit na uhog upang salain ang alikabok, pollen, at iba pang dumi sa hangin.") },
        { "tracheaDescriptionCon", ("Trachea is known as the windpipe. It also filters the air we inhale and branches into the bronchi.", "Ang trachea ay kilala bilang windpipe. Sinasala rin nito ang hangin na nilalanghap at nahahati ito papunta sa bronchi.") },
        { "lungsDescriptionCon", ("Lungs are the main organ of the respiratory system. This is where exchange of gases occurs, oxygen is taken in and carbon dioxide is expelled out.", "Ang baga ang pangunahing bahagi ng respiratory system. Dito nagaganap ang palitan ng hangin: pumapasok ang oxygen at inilalabas ang carbon dioxide.") },
        { "bronchiDescriptionCon", ("Bronchi are two tubes that carry air into the lungs. Bronchial tubes branch into smaller tubes called bronchioles.", "Ang bronchi ay dalawang tubo na nagdadala ng hangin papunta sa baga. Nahahati ito sa mas maliliit na tubo na tinatawag na bronchioles.") },
        { "larynxDescriptionCon", ("The larynx contains two vocal cords that vibrate when air passes by them.", "Ang larynx ay may dalawang vocal cords na gumagalaw kapag dumadaan ang hangin, na siyang lumilikha ng tunog.") },

        // Nervous
        { "cerebrumDescriptionCon", ("The largest part of the brain. This part receives sensory messages. It acts as the center of emotions, consciousness, learning and voluntary movement.", "Ang pinakamalaking bahagi ng utak. Tumatanggap ito ng mga mensahe mula sa mga pandama. Ito rin ang sentro ng emosyon, kamalayan, pagkatuto, at kusang galaw.") },
        { "hypothalamusDescriptionCon", ("A small but vital part of the brain. It helps control the body’s internal balance. This part regulates hunger, thirst, body temperature, and sleep. It also plays a key role in emotions and hormone release.", "Isang maliit ngunit mahalagang bahagi ng utak. Tinutulungan nitong panatilihing balanse ang loob ng katawan. Kinokontrol nito ang gutom, uhaw, temperatura ng katawan, at tulog. Mahalaga rin ito sa damdamin at sa pagpapalabas ng mga hormone.") },
        { "medullaOblongataDescriptionCon", ("The lowest part of the brainstem. This part controls automatic body functions. It helps manage breathing, heart rate, blood pressure, and swallowing without you thinking about it.", "Ang pinakailalim na bahagi ng brainstem. Kinokontrol nito ang mga awtomatikong gawain ng katawan gaya ng paghinga, tibok ng puso, presyon ng dugo, at paglunok kahit hindi mo ito iniisip.") },
        { "cerebellumDescriptionCon", ("Located beneath the cerebrum. It is smaller than the cerebrum. It coordinates involuntary and muscle action. It is responsible for man's ability to learn habits and develop skills. It also helps maintain a person's sense of balance.", "Matatagpuan sa ilalim ng cerebrum at mas maliit kaysa rito. Pinagkokoordinado nito ang mga kilos ng kalamnan at mga hindi sinasadyang galaw. Responsable ito sa pagkatuto ng mga gawi, kasanayan, at pagpapanatili ng balanse.") },
        { "brainStemDescriptionCon", ("The elongated area at the base of the brain. It contains vital centers for autonomic functions.", "Ang pahabang bahagi sa ibaba ng utak. Taglay nito ang mga mahalagang sentro para sa awtomatikong gawain ng katawan.") },

        // Circulatory
        { "bloodVesselsDescriptionCon", ("The blood vessels are the vast networks of small tubes that carry blood throughout the body. The arteries are blood vessels that carry oxygen-rich blood away from the heart. Veins carry deoxygenated blood back to the heart. The capillaries are the smallest blood vessels which serve as a connection between arteries and veins. When blood passes through them, oxygen, food nutrients and wastes pass in and out through capillary walls.", "Ang mga daluyan ng dugo ay mga maliliit na tubo na bumubuo ng malawak na network sa buong katawan. Ang mga ugat ay naghahatid ng dugo mula at pabalik sa puso. Ang mga ugat na may oxygen (arteries) ay nagdadala ng dugo mula sa puso. Ang mga ugat na walang oxygen (veins) ay nagbabalik ng dugo sa puso. Ang mga capillary naman ang pinakamaliit na daluyan ng dugo na koneksyon ng arteries at veins at dito nagaganap ang palitan ng oxygen, nutrients, at basura.") },
        { "veinsDescriptionCon", ("Veins are blood vessels that carry deoxygenated blood back toward the heart, except for the pulmonary veins, which carry oxygenated blood from the lungs. They have thinner walls than arteries and contain valves that prevent blood from flowing backward. Veins rely on surrounding muscles to help push the blood upward, especially from the lower parts of the body. They are often located closer to the surface of the skin and appear blue due to the way light reflects off the skin.", "Ang mga ugat ay daluyan ng dugo na nagdadala ng dugo na kulang sa oxygen pabalik sa puso, maliban sa pulmonary veins na may dalang oxygen mula sa baga. Mas manipis ang dingding ng mga ugat kaysa sa arteries at may mga balbula ito upang pigilan ang pagbalik ng dugo. Umaasa ito sa paggalaw ng kalamnan upang itulak pataas ang dugo, lalo na mula sa ibabang bahagi ng katawan. Karaniwang makikita ito malapit sa balat at mukhang asul dahil sa repleksyon ng liwanag.") },
        { "capillariesDescriptionCon", ("Capillaries are the smallest and thinnest blood vessels in the body. They form a network that connects arteries to veins, allowing for the exchange of gases, nutrients, and waste products between the blood and body tissues. Oxygen and nutrients pass through their thin walls into the cells, while carbon dioxide and waste products move from the cells into the capillaries. Because of their size, red blood cells must pass through them in single file, making capillaries essential for efficient nutrient and gas exchange.", "Ang mga capillary ang pinakamaliit at pinakamanipis na daluyan ng dugo sa katawan. Nag-uugnay ito sa arteries at veins at dito nagaganap ang palitan ng oxygen, nutrients, at mga basura ng katawan. Dahil sa nipis ng pader nito, kayang makalusot ang oxygen at sustansya papasok sa cells habang lumalabas naman ang carbon dioxide at basura. Dahil sa sukat nito, kailangang dumaan ang red blood cells ng paisa-isa.") },
        { "arteriesDescriptionCon", ("Arteries are thick-walled blood vessels that carry oxygen-rich blood away from the heart to the body, except for the pulmonary artery, which carries deoxygenated blood to the lungs. Their muscular and elastic walls allow them to handle the high pressure of blood being pumped by the heart. The largest artery is the aorta, which branches out into smaller arteries that reach different organs and tissues. Arteries play a vital role in maintaining blood pressure and ensuring that oxygen and nutrients are delivered efficiently throughout the body.", "Ang mga artery ay makakapal na daluyan ng dugo na nagdadala ng dugo na may oxygen mula sa puso papunta sa buong katawan, maliban sa pulmonary artery na papunta sa baga. Dahil sa makakapal at elastic na pader, kaya nitong taglayin ang mataas na presyon mula sa pagtibok ng puso. Ang pinakamalaking artery ay ang aorta na naghahati-hati patungo sa iba't ibang bahagi ng katawan. Mahalaga ang arteries sa pagpapanatili ng presyon ng dugo at paghahatid ng oxygen at nutrisyon.") },
        { "heartDescriptionCon", ("The heart is known as the pumping organ of the body. It keeps the blood moving throughout the body and the average heartbeat of a human is 60 to 100 times per minute. It has four chambers: the left and right atrium which are responsible for receiving used blood coming from all parts of the body and the left and right ventricles known as the pumping chambers. When its contracts, oxygen-rich blood is forced away from the heart for the distribution to the different parts of the body. Between atrium and ventricles are valves, the overlapping tissue that allows blood to flow in one direction.", "Ang puso ay ang nagpapadaloy ng dugo sa buong katawan. Tumitibok ito nang 60 hanggang 100 beses bawat minuto. Mayroon itong apat na silid: kaliwa at kanang atrium na tumatanggap ng nagamit na dugo, at kaliwa at kanang ventricle na nagpapapump ng dugo. Kapag ito ay humigpit, tinutulak nito ang dugo na may oxygen papunta sa katawan. May mga balbula sa pagitan ng atrium at ventricle upang matiyak na isang direksyon lang ang agos ng dugo.") },

        // Circulatory - Heart Chambers & Valves
        { "rightVentricleDescriptionCon", ("Pumps oxygen-poor blood to the lungs through the pulmonary artery.", "Nagpapapump ng dugo na kulang sa oxygen papunta sa baga sa pamamagitan ng pulmonary artery.") },
        { "rightAtriumDescriptionCon", ("Receives oxygen-poor blood from the body through the superior and inferior vena cava.", "Tumatanggap ng dugo na kulang sa oxygen mula sa katawan sa pamamagitan ng superior at inferior vena cava.") },
        { "leftAtriumDescriptionCon", ("Receives oxygen-rich blood from the lungs through the pulmonary veins.", "Tumatanggap ng dugo na mayaman sa oxygen mula sa baga sa pamamagitan ng pulmonary veins.") },
        { "leftVentricleDescriptionCon", ("Pumps oxygen-rich blood to the body through the aorta.", "Nagpapapump ng dugo na mayaman sa oxygen papunta sa katawan sa pamamagitan ng aorta.") },
        { "superiorVenaCavaDescriptionCon", ("Brings oxygen-poor blood from the upper body to the right atrium.", "Nagdadala ng dugo na kulang sa oxygen mula sa itaas na bahagi ng katawan papunta sa right atrium.") },
        { "inferiorVenaCavaDescriptionCon", ("Brings oxygen-poor blood from the lower body to the right atrium.", "Nagdadala ng dugo na kulang sa oxygen mula sa ibabang bahagi ng katawan papunta sa right atrium.") },
        { "pulmonaryVeinDescriptionCon", ("Brings oxygen-rich blood from the lungs to the left atrium.", "Nagdadala ng dugo na mayaman sa oxygen mula sa baga papunta sa left atrium.") },
        { "pulmonaryArteryDescriptionCon", ("Carries oxygen-poor blood from the right ventricle to the lungs.", "Nagdadala ng dugo na kulang sa oxygen mula sa right ventricle papunta sa baga.") },
        { "tricuspidValveDescriptionCon", ("Keeps blood from flowing back into the right atrium when the right ventricle contracts.", "Pinipigilan ang pagbalik ng dugo sa right atrium kapag humihigpit ang right ventricle.") },
        { "pulmonaryValveDescriptionCon", ("Controls blood flow from the right ventricle into the pulmonary artery.", "Kinokontrol ang daloy ng dugo mula sa right ventricle papunta sa pulmonary artery.") },
        { "mitralValveDescriptionCon", ("Prevents backflow of blood from the left ventricle to the left atrium.", "Pinipigilan ang pagbalik ng dugo mula sa left ventricle papunta sa left atrium.") },
        { "aorticValveDescriptionCon", ("Regulates blood flow from the left ventricle into the aorta.", "Kinokontrol ang daloy ng dugo mula sa left ventricle papunta sa aorta.") },


        //Excretory
        { "kidneysDescriptionCon", ("The kidneys are two bean-shaped organs on each side of the body. They are the main organs of the urinary system. The kidneys filter and clean the blood to remove waste substance and ensure that only useful substances stay in the blood that circulates throughout the body. The kidneys remove excess water, salts, urea, and other wastes from the blood. Each kidney has thousands of filters called nephrons that filter the blood about 300 times a day.",
         "Ang mga bato ay dalawang hugis-beans na bahagi na matatagpuan sa magkabilang gilid ng katawan. Ito ang pangunahing bahagi ng sistemang urinari. Ang mga bato ang nagsasala at naglilinis ng dugo upang alisin ang mga basurang sangkap at tiyakin na ang mga kapaki-pakinabang lamang ang mananatili sa dugong umiikot sa buong katawan. Inaalis ng mga bato ang sobrang tubig, asin, urea, at iba pang basura mula sa dugo. Ang bawat bato ay may libo-libong maliliit na tagasala na tinatawag na nephrons na nagsasala ng dugo humigit-kumulang 300 beses sa isang araw.") },
        {"uretersDescriptionCon",("The urine flows out from the kidneys to the urinary bladder through the two ureters. The smooth muscles of the ureters move by peristalsis to move the urine towards the urinary bladder.", "Ang ihi ay dumadaloy mula sa mga bato patungo sa pantog sa pamamagitan ng dalawang ureter.Ang mga makikinis na kalamnan ng ureter ay gumagalaw sa pamamagitan ng peristalsis upang itulak ang ihi papunta sa urinary bladder.")},
        {"urinaryBladderDescriptionCon",("The urinary bladder is a hollow muscular organ where urine is temporarily stored. The muscles of the bladder contracts to push the urine out of the body.", "Ang pantog ay isang hungkag at masel na bahagi ng katawan kung saan pansamantalang iniimbak ang ihi. Kapag puno na, umiimpis ang mga kalamnan ng pantog upang itulak palabas ang ihi.")},
        {"urethraDescriptionCon",("Urine is emptied from the bladder and leaves the body through the urethra. The urethra is a tube at the bottom of the bladder. The length of the urethra varies in males and females. The urethra has a sphincter muscle which serves as a valvelike structure to help regulate the outflow of urine.", "Ang ihi ay inilalabas mula sa pantog at lumalabas sa katawan sa pamamagitan ng urethra. Ang urethra ay isang tubo na nasa ibaba ng pantog. Nagkakaiba ang haba nito sa lalaki at babae. May sphincter muscle ang urethra na nagsisilbing parang balbula na kumokontrol sa pagdaloy ng ihi palabas ng katawan.")},
    };



    public static Dictionary<string, (string enDidYouKnow, string tlDidYouKnow, string enHeader,string tlHeader, string enDescription, string tlDescription)> FunFacstsText = new()
    {

        { "funFactsCard1", ("Did You Know...", "Alam Mo Ba...", "Your small intestine is longer than you are!",
        "Mas mahaba pa ang maliit mong bituka kaysa sa’yo!",
         "It’s about 7 meters long, that’s like 3 classroom tables in a row!",
         "Mga 7 metro ang haba nito — parang 3 mesa sa classroom na magkakasunod!" )},
        
        { "funFactsCard2", ("Did You Know...", "Alam Mo Ba...","Your stomach makes acid strong enough to melt metal.",
        "Gumagawa ang tiyan mo ng asido na kayang tunawin ang metal.",
         "But don’t worry! Your stomach has a special lining to protect itself",
         "Pero huwag kang mag-alala! May espesyal na panangga ang tiyan para hindi ito masira." )},
        
        { "funFactsCard3", ("Did You Know...", "Alam Mo Ba...","It takes 24 to 72 hours to fully digest a meal.",
        "Umaabot ng 24 hanggang 72 oras bago tuluyang matunaw ang pagkain.",
         "That’s 1 to 3 whole days from bite to bathroom!",
         "Ibig sabihin, 1 hanggang 3 araw mula sa unang subo hanggang sa pagpunta sa banyo!" )},

        { "funFactsCard4", ("Did You Know...", "Alam Mo Ba...", "You produce about 1.5 liters of saliva every day.",
        "Gumagawa ka ng humigit-kumulang 1.5 litro ng laway araw-araw.",
         "That’s almost as much as a big soda bottle!",
         "Halos kasing dami ng laman ng malaking bote ng softdrinks!" )},

        { "funFactsCard5", ("Did You Know...", "Alam Mo Ba...", "Food doesn’t just fall down your throat, muscles push it down!",
        "Hindi basta-basta nalalaglag ang pagkain sa lalamunan mo, tinutulak ito ng mga kalamnan!",
        "This movement is called peristalsis.",
        "Ang tawag dito ay peristalsis.")}, 
        
          { "funFactsCard6", ("Did You Know...", "Alam Mo Ba...", "The small intestine is called “small” only because it’s thinner.",
        "Tinawag lang na 'maliit' ang small intestine dahil mas manipis ito.",
        "It’s actually much longer than the large intestine!",
        "Pero mas mahaba ito kaysa sa large intestine!")}, 


           { "funFactsCard7", ("Did You Know...", "Alam Mo Ba...", "Your digestive system starts working even before you eat.",
        "Umiikot na ang digestive system mo kahit bago ka pa kumain.",
        "Just smelling food can make your mouth produce saliva.",   
        "Sa amoy pa lang ng pagkain, nagpapalabas na ng laway ang bibig mo.")}, 


          { "funFactsCard8", ("Did You Know...", "Alam Mo Ba...", "You fart because of digestion!",
        "Nagkaka-utot ka dahil sa pagtunaw ng pagkain!",
        "Gases are made when bacteria break down food in your intestines.",
        "Nagkakaroon ng gas kapag binabasag ng mga bacteria ang pagkain sa bituka mo."
        )}, 


          { "funFactsCard9", ("Did You Know...", "Alam Mo Ba...", "It takes about 6 to 8 hours for food to reach your small intestine.",
        "Umaabot ng 6 hanggang 8 oras bago makarating ang pagkain sa small intestine.",
        "The stomach holds it for a while to mix and mash it first.",
        "Pinipigil muna ito ng tiyan para haluin at durugin.")}, 


          { "funFactsCard10", ("Did You Know...", "Alam Mo Ba...", "Liver, gallbladder, and pancreas are part of digestion too!",
        "Kasama rin sa pagtunaw ng pagkain ang atay, apdo, at lapay!",
        "Even though food doesn’t pass through them, they help a lot with breaking it down.",
        "Kahit hindi dumadaan doon ang pagkain, tumutulong silang durugin ito.")}, 

    };
    public static Dictionary<string, (string enNeuronName, string enDescription, string tlNeuronName, string tlDescription)> NeuronsCardText = new()
    {   
      { "neuronsCard1", ("Nerve Cell",
      "The neuron or nerve cell is the functional unit of the nervous system. The neuron has three parts.",
      "Selulang Nerbiyo",
      "Ang neuron o selulang nerbiyo ang pangunahing yunit ng nervous system. May tatlong bahagi ang neuron.")},

      { "neuronsCard2", ("Cell Body",
      "The cell body is the main component of the neuron. It maintains the health of the neuron.",
      "Katawan ng Selula",
      "Ang katawan ng selula ang pangunahing bahagi ng neuron. Pinananatili nito ang kalusugan ng neuron."
      )},

      { "neuronsCard3", ("Dendrites",
      "The dendrites are the short fibers around the cell body. They carry messages into the nerve cell.",
      "Dendrite",
      "Ang mga dendrite ay maikling hibla sa paligid ng katawan ng selula. Dinadala nila ang mga mensahe papasok sa selulang nerbiyo.")},

      { "neuronsCard4", ("Axon",
      "The axon is the long fiber of the neuron.",
      "Axon",
      "Ang axon ay ang mahabang hibla ng neuron.")},

    };
}
