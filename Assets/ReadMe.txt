Guide Untuk Menggunakan Module Dialogue:

1. User Dialogue : menyimpan data nama orang yang melakukan dialogue, serta gambar (potrait) dari orang tersebut (jika ada, kalo tidak ada bisa di ignore).

1. Untuk Dialogue Dengan Text Biasa: 
- Masukkan list kalimat yang ingin dibicarakan dalam satu kali jalan. 
- Masukkan informasi User Dialogue kedalam dialogue tersebut.
- Pilih Type menjadi Text Only

2. Untuk Dialogue Dengan Pilihan :
- Pilih Type menjadi Choice
- Masukkan list kalimat yang ingin dibicarakan.
- Perhatikan bahwa di choice, pilihan akan ke trigger sehabis kalimat pertama. 
- Jika sehabis pilihan, Jika ingin memasukan nama pilihan kedalam dialogue, dapat menggunakan {pokemon}

Dialogue Manager :
- TextSpeed semakin kecil semakin cepat.
- GameObject disesuaikan dengan contoh.
- Dapat memasukkan dialogue yang ingin dimasukkan kedalam list OriginalDialogues

Catatan : 
- Jika ingin menggabungkan text dan choice, bisa terpisah menjadi 2 dialogue, dialogue pertama adalah dialogue text only, dan dialogue kedua adalah dialogue choice.
- Sudah disediakan event OnDialogueEnd, untuk nge trigger event setelah dialogue selesai dilakukan.
- Sudah disediakan event OnPicked didalem choice, untuk nge trigger event setelah choice tersebut dipilih.
- Sudah disediakan Dialogue Database, ini akan menjadi list dialogue didalam game nantinya, jadi ketika ingin mentrigger dialogue, 
tinggal memberitahu index dialoguedatabase mana yang akan ke trigger, dan otomatis dimasukkan ke dialogue di dialoguemanager.
