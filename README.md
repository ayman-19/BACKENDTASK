Approach: How did you approach the concurrency and memory constraints?

	1- قراءة ملف ضخم جدا (10GB+)
	2- بدون ما اكسر البرنامج بسبب الذاكرة
	3- ومع استغلال كامل للـ CPU

   usage:
	* I use Streaming + Batching + Parallel Processing.
	* I read file line by line using FileStream and StreamReader
	* تجميع كل 100000 سطر في Batch
	* معالجة كل Batch باستخدام Parallel.ForEach
	* تخزين العد داخل ConcurrentDictionary

Selection: Why did you choose these specific data structures and threading models?

	1- why use FileStream + StreamReader? => قراءة Streaming بدون تحميل الملف بالكامل
	2- why use List<string> (Batching)? => تقليل الضغط على الـ CPU
	3- why use Parallel.ForEach? => Utilizing all processor cores
	4- why use ConcurrentDictionary? => Safe counter without race conditions

Trade-offs: What are the upsides and downsides of your specific implementation?

    pros:
	 * استهلاك ثابت للذاكرة	
	 * أمان في التعدد (Thread-Safe)
	 * يشتغل على ملفات ضخمة جدا
	 * رامات قليلة
	 * سريع

    cons:
	 * استهلاك عالي للـ CPU

Alternatives: What other approaches did you consider, and why did you decide against them?

	1- File.ReadAllLines => يستهلك الرام بالكامل لان بيحمل كل البيانات مره واحده علي memory
	2- Single Thread => أداء ضعيف
	3- Dictionary => لأنه يعمل Race Conditions
