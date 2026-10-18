🛒 مشروع ECommerce API

تطبيق طلبات صغير (Backend API): اليوزر بيتصفح المنتجات، وبيحط اللي عايزه في السلة (Basket)، وبعدين بيدفع أونلاين بالفيزا. المشروع مبني بـ ASP.NET Core على نظام Onion Architecture، ومعاه لوحة تحكم (Admin Dashboard) لإدارة المنتجات وصلاحيات المستخدمين.

الريبو ده لعرض اللي عملته في المشروع وشرح فكرته، ومش معمول للتجربة أو التشغيل. ومفيش Frontend، فالـ API بيتعرض من Swagger.

<img width="921" height="943" alt="Screenshot 2026-10-18 154647" src="https://github.com/user-attachments/assets/1fcd0d61-771b-4e2e-9f43-276df0850bde" />


✨ اللي بيعمله المشروع

🔐 التسجيل والدخول (Authentication): بـ JWT مع ASP.NET Core Identity. تسجيل يوزر جديد، دخول، بيانات اليوزر الحالي، التأكد من الإيميل، عنوان اليوزر، ونسيت أو إعادة تعيين كلمة المرور.

🛍️ المنتجات: عرض المنتجات مع Pagination وفلترة وترتيب باستخدام Specification Pattern.

🧺 سلة المشتريات: إنشاء وتحديث وحذف السلة، ومتخزنة في Redis.

📦 الأوردرات: إنشاء الأوردر وعرض أوردرات اليوزر وطرق التوصيل المتاحة.

💳 الدفع الأونلاين: بالفيزا عن طريق Stripe مع استقبال Webhook، والأسعار بتتحسب من الداتا بيز مش من الفرونت.

⚡ الـ Cache: بـ Redis لتقليل الضغط على قاعدة البيانات.

✉️ الإيميلات: بيتبعت لما يوزر جديد يسجل، أو يحدّث بياناته، أو يغيّر كلمة المرور، وكمان لما الدفع ينجح بتفاصيل الأوردر كاملة.

🛠️ لوحة التحكم: مشروع ويب منفصل لعرض المنتجات، وإضافة منتج برفع صورته وحذفه، وإعطاء صلاحيات للمستخدمين زي Admin.

🧱 معالجة الأخطاء: بتتعالج في مكان واحد. Middleware مخصص للـ Exceptions، وResult Pattern بيرجّع الأخطاء المتوقعة (زي NotFound وValidation) بشكل موحد، وشكل موحد لأخطاء الـ Validation.

🌱 تعبئة الداتا الابتدائية (Data Seeding): المنتجات واليوزرز بتتحط تلقائيًا عند أول تشغيل.

🧰 التقنيات المستخدمة

الـ Backend بـ ASP.NET Core Web API وEntity Framework Core، وقاعدة البيانات SQL Server (قاعدتين منفصلتين: واحدة للمتجر وواحدة للـ Identity)، وRedis للسلة والـ Cache، وJWT مع ASP.NET Core Identity للـ Authentication، وStripe للدفع، وSMTP للإيميلات، وSwagger للتوثيق، وAutoMapper للـ Mapping. وبنيته على Unit of Work وRepository وSpecification Pattern.

🏗️ الـ Architecture

المشروع مبني بنظام Onion Architecture: الـ Domain في النص ومبيعرفش حاجة عن باقي الطبقات، وكل الـ Dependencies بتتجه للداخل. الطبقات:

🧅 طبقة الـ Core (ApplicationCoreLayer): فيها ECommerce.Doamin للـ Entities والـ Contracts، وECommerce.Service للـ Business Logic والـ Specifications والـ MappingProfiles، وECommerce.Services.Abstraction للـ Interfaces بتاعة الخدمات.

🗄️ طبقة الـ Infrastructure: فيها ECommerce.Presistence للـ DbContexts والـ Repositories وRedis والـ EmailService.

🌐 طبقة الـ Presentation: فيها ECommerce.Api (Program.cs والـ Middlewares) وECommerce.Presentation (الـ Controllers).

🖥️ طبقة الـ UI: فيها لوحة التحكم AdminDashBoard.

📎 مشروع ECommerce.Shared: فيه الـ DTOs والـ PaginatedResult وبارامترات البحث والترتيب.

⚡ ريديس (Redis) بيعمل إيه في المشروع؟

ريديس قاعدة بيانات بتشتغل في الذاكرة (RAM) بدل الهارد، فهي أسرع بكتير من SQL Server، وبتخزن بيانات بسيطة على شكل مفتاح وقيمة (Key / Value). استخدمته في حاجتين:

🧺 السلة (Basket): لما اليوزر بيضيف منتج أو بيشيله أو بيغير الكمية، السلة بتتحدث كتير ورا بعض. لو كل تعديل بيتكتب في SQL Server هيبقى ضغط على الداتا بيز عشان حاجة لسه مخلصتش. فالسلة بتتحفظ في Redis بمفتاح هو الـ basketId، ولما اليوزر يعمل الأوردر فعلًا بيتسجل في SQL Server.

🗂️ الـ Cache: النتايج اللي بتتطلب كتير بتتخزن في Redis، فالطلبات المتكررة بتتجاوب منه من غير ما تروح للداتا بيز.

النتيجة إن SQL Server مبيشتغلش غير في الحاجات اللي لازم تتحفظ فعلًا.

💳 الدفع الأونلاين (Stripe) والـ Webhook

الفكرة الأساسية إن بيانات الكارت عمرها ما بتعدي على الـ Backend بتاعي. اليوزر بيدخلها في الفرونت اللي بيكلم Stripe مباشرة، والـ Backend بيجهّز الدفع ويستنى Stripe يبلّغه بالنتيجة. الخطوات بالترتيب:

1️⃣ الفرونت بيبعت للـ Backend طلب تجهيز الدفع (POST /api/Payments/BasketId) ومعاه رقم السلة بس، مش المبلغ. الـ Backend بيجيب السلة من Redis، ويحسب سعر كل منتج وسعر التوصيل من الداتا بيز (عشان محدش يعدّل السعر من الفرونت)، ويحسب المبلغ.

2️⃣ الـ Backend بيطلب من Stripe يعمل Payment Intent بالمبلغ ده. لو السلة عندها Payment Intent من قبل (اليوزر غيّر الكمية مثلًا) بيحدّث مبلغه بدل ما يعمل واحد جديد. وبيحفظ الـ PaymentIntentId والـ ClientSecret في السلة (في Redis) ويرجّعهم للفرونت.

3️⃣ الفرونت ياخد الـ ClientSecret ويفتح فورم الكارت بتاع Stripe. اليوزر بيكتب بيانات الكارت وبتتبعت لـ Stripe نفسه، ويأكد الدفع.

4️⃣ الأوردر بيتسجل في الـ Backend (POST /api/Orders) ومربوط بالـ PaymentIntentId بتاع السلة، وده اللي بيخلّينا نعرف نربط الدفع بالأوردر بعدين.

5️⃣ بعد ما Stripe يخلص الدفع، هو اللي بيبعت Webhook للـ Backend (POST /api/Payments/webhook) بالنتيجة. الـ Backend بيتأكد إن الطلب جاي من Stripe فعلًا عن طريق التوقيع (Stripe-Signature) والـ Endpoint Secret، وبعدين بيدوّر على الأوردر بالـ PaymentIntentId.

6️⃣ لو الدفع نجح، حالة الأوردر بتبقى PaymentReceived ويتبعت لليوزر إيميل بتفاصيل الأوردر. لو فشل، حالته بتبقى PaymentFailed.

ليه Webhook؟ لأن الدفع بيحصل بين اليوزر وStripe من غير الـ Backend. فلو الـ Backend صدّق الفرونت وهو بيقول "أنا دفعت"، أي حد ممكن يكدب. الـ Webhook هو الطريقة الموثوقة إن Stripe نفسه هو اللي يقول النتيجة.

🧠 أهم التحديات

💳 الدفع والـ Webhook: أكبر تحدي كان فهم الـ Flow بين الفرونت والـ Backend وStripe (موضح فوق)، والتأكد إن الـ Webhook جاي فعلًا من Stripe.

🔎 الـ Specification Pattern: بدل ما منطق الفلترة والترتيب والـ Include والـ Pagination يتكرر جوه الـ Repository، بيتكتب في كلاسات صغيرة (Specifications) والـ SpecificationEvaluator بيطبقها على الـ Query. حتى البحث عن الأوردر بالـ PaymentIntentId في الـ Webhook بيتم بـ Specification.

🗃️ قاعدتين بيانات منفصلتين: واحدة للمنتجات والأوردرات (StoreDbContext) وواحدة للـ Identity (StoreIdentityDbContext)، وكل واحدة ليها Migrations والـ Seed بتاعها.

📌 ملاحظة

مشروع تعليمي وتجريبي، ومفيش فيه يوزرز حقيقيين.
