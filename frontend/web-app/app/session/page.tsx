import { auth } from "@/auth";
import React from "react";
import Heading from "../components/Heading";
import AuthTest from "./AuthTest";

export default async function Session() {
  const session = await auth();
  return (
    <div>
      <Heading title="Session dashboard" />

      <div className="bg-blue-200 border-2 border-blue-500 m-2.5 w-full p-1.5">
        <h3 className="text-lg">Session data</h3>
        <pre className="whitespace-pre-wrap break-all">
          {JSON.stringify(session?.user, null, 2)}
        </pre>
      </div>
      <div className="bg-green-200 border-2 border-green-500 m-2.5 w-full p-1.5">
        <h3 className="text-lg">Token data</h3>
        <pre className="whitespace-pre-wrap break-all">
          {JSON.stringify(session?.token, null, 2)}
        </pre>
      </div>
      <div className="mt-4">
        <AuthTest />
      </div>
    </div>
  );
}
