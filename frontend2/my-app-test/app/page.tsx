"use client";

import { useEffect, useState } from "react";

export default function Home() {
  const baseurl = process.env.NEXT_PUBLIC_API_URL;
  const [auctions, setAuctions] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch(`${baseurl}/search`, {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
          },
        });
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        const data = await response.json();
        setAuctions(data);
        console.log("data", data);
      } catch (error) {
        console.error("There was a problem with the fetch operation:", error);
      }
    };

    fetchData();
  }, []);

  return (
    <>
      <div className="grid grid-rows-[20px_1fr_20px] items-center justify-items-center min-h-screen p-8 pb-20 gap-16 sm:p-20 font-[family-name:var(--font-geist-sans)]">
        baseurl: {baseurl}
      </div>
      <div>
        string
        {JSON.stringify(auctions)}
      </div>
    </>
  );
}
